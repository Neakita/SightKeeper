/*
using System.Diagnostics;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using SightKeeper.Application.Training;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Services.Training;

public sealed class DetectorTrainer : ReactiveObject, ModelTrainer<DetectorModel>
{
    private const string DarknetFolder = "Training/Darknet";
    private const string BackupFolder = $"{DarknetFolder}/backup";
    private const string ImagesDataFolder = $"{DarknetFolder}/data/train";
    private const string NamesFilePath = $"{DarknetFolder}/data/names.txt";
    private const string LastWeightsFilePath = $"{DarknetFolder}/backup/last.weights";
    private const string DataFilePath = $"{DarknetFolder}/data/data.txt";
    
    
	[Reactive] public bool IsRunning { get; private set; }
    [ObservableAsProperty] public float? Progress { get; }
    [Reactive] public uint? CurrentBatch { get; private set; }
    [Reactive] public uint? MaxBatches { get; private set; }
    [Reactive] public string Status { get; private set; }
    [Reactive] public double? AverageLoss { get; private set; }
    [ObservableAsProperty] public TimeSpan? TimeRemaining { get; }


    public DetectorTrainer()
    {
        this.WhenAnyValue(
                t => t.CurrentBatch,
                t => t.MaxBatches)
            .Select(t => (float?)t.Item1 / t.Item2)
            .ToPropertyEx(this, t => t.Progress);
    }

    public void BeginTraining(DetectorModel model, bool fromScratch)
    {
        throw new NotImplementedException();
    }

    public void EndTraining()
	{
		throw new NotImplementedException();
	}

    private Process? _darknetProcess;
    private DateTime? _lastBatchPassTime;
    private DetectorModel? _model;
    private readonly List<TimeSpan> _timeRemainingHistory = new();
    private DarknetConsoleParser _consoleParser;

    public void Start()
    {
        IsRunning = true;
        Train();
    }

    public void Stop()
    {
        IsRunning = false;
        TerminateProcess();
        SaveResult();
    }

    private void Train()
    {
        Status = "Preparing folder";
        ClearFolder();
        PrepareFolder();
        Status = "Preparing images";
        PrepareImages();
        Status = "Preparing classes list";
        PrepareClassesList();
        Status = "Preparing data file";
        PrepareData();
        Status = "Preparing config";
        PrepareConfig();
        Status = "Starting the process";
        RunProcess();
    }

    private void PrepareFolder(bool scratchTraining)
    {
        if (!Directory.Exists(BackupFolder)) Directory.CreateDirectory(BackupFolder);
        if (!Directory.Exists(ImagesDataFolder)) Directory.CreateDirectory(ImagesDataFolder);
        string modelWeightsPath = "Data/Models/" + ModelForTraining.Id + "/last.weights";
        var backupLastWeights = "Data/Trainer/Darknet/backup/last.weights";
        // TODO materialize from db
        if (!scratchTraining && File.Exists(modelWeightsPath)) File.Copy(modelWeightsPath, backupLastWeights, true);
        else File.Delete(backupLastWeights);
    }
    
    private void PrepareImages()
    {
        var destinationFolder = "Data/Trainer/Darknet/data/train";

        ModelForTraining.ExportAssets(destinationFolder);

        using (var imagesListStream = new StreamWriter(Path.Combine(DarknetFolder, "data", "train.txt"), false))
        {
            foreach (var filePath in Directory.GetFiles(destinationFolder, "*.png")) imagesListStream.WriteLine("data/train/" + Path.GetFileName(filePath));
        }
    }

    private void PrepareClassesList()
    {
        if (_model == null) throw new NullReferenceException("Model is null");
        File.WriteAllText(NamesFilePath, string.Join('\n', _model.ItemClasses));
    }

    private void PrepareData()
    {
        if (_model == null) throw new NullReferenceException("Model is null");
        using StreamWriter stream = new(Path.Combine(DarknetFolder, "data", "data.txt"), false);
        stream.WriteLine($"classes={_model.ItemClasses.Count}");
        stream.WriteLine("train=data/train.txt");
        stream.WriteLine("labels = data/names.txt");
        stream.WriteLine("backup = backup/");
        stream.WriteLine("top=2");
    }

    private void PrepareConfig()
    {
        var fileDestName = Path.Combine(DarknetFolder, "data", "config.txt");
        var textFromConfigFile = File.ReadAllText(Path.Combine(
            ModelForTraining.GetType() == typeof(DetectorModel)
                ? FileSystem.DetectorFilesDirectory.FullName
                : FileSystem.ClassifierFilesDirectory.FullName, ModelForTraining.Config));

        using var stream = new StreamWriter(fileDestName, false);
        stream.Write(DarknetConfigProvider.ReplaceForTraining(textFromConfigFile,
            ModelForTraining.TrainingResolution.Size.Width, ModelForTraining.TrainingResolution.Size.Height,
            ModelForTraining.Classes.Count));

        MaxBatches = DarknetConfigProvider.CalculateMaxBatches(ClassesCount);
    }

    private void RunProcess()
    {
        string arg = $@"{ModelForTraining.ModelType.ToLower()} train data/data.txt data/config.txt" +
                     (File.Exists(Path.Combine(DarknetFolder, "backup", "last.weights"))
                         ? " backup/last.weights"
                         : string.Empty) + " -dont_show";
        
        ProcessStartInfo processStartInfo = new(Path.Combine(DarknetFolder, "darknet.exe"), arg) 
        {
            WorkingDirectory = DarknetFolder, 
            RedirectStandardOutput = true,
            CreateNoWindow = false,
            UseShellExecute = false,
            RedirectStandardInput = false,
            RedirectStandardError = false
        };
        _darknetProcess = Process.Start(processStartInfo);
        _consoleParser = ModelForTraining.GetType() == typeof(DetectorModel)
            ? new DetectorConsoleParser(_darknetProcess)
            : new ClassifierConsoleParser(_darknetProcess);
        
        _consoleParser.BatchPassed += OnBatchPassed;
        
        Application.Current.Dispatcher.Invoke(() => { Application.Current.Exit += OnExit; });

        _darknetProcess.BeginOutputReadLine();
        //_darknetProcess.BeginErrorReadLine();

        _timeoutTimer = new Timer(OnTimeout, null, 120000, Timeout.Infinite);
    }

    private void OnBatchPassed(int batch, double averageLoss)
    {
        if (batch % 100 == 0)
        {
            while (!File.Exists("Data/Trainer/Darknet/backup/config_last.weights")) Thread.Sleep(1);
            SaveResult();
        }
        
        Status += CalculateRemainingTime();
        
        if (CurrentBatch == MaxBatches) Stop();
        else
        {
            _timeoutTimer.Change(120000, Timeout.Infinite);
        }
        
    }

    private Timer _timeoutTimer;

    private void OnExit(object sender, ExitEventArgs e)
    {
        if (_darknetProcess == null) return;
        SaveResult();
        TerminateProcess();
    }

    private static void OnTimeout(object? state)
    {
        throw new TimeoutException();
    }

    private string CalculateRemainingTime()
    {
        string result = string.Empty;
        if (_lastBatchPassTime != DateTime.MinValue)
        {
            _timeRemainingHistory.Add(DateTime.UtcNow - _lastBatchPassTime);
            if (_timeRemainingHistory.Count > 500) _timeRemainingHistory.RemoveAt(0);
            ulong totalSeconds = 0;
            foreach (var item in _timeRemainingHistory) totalSeconds += (ulong) item.TotalSeconds;
            var avgElapsedSeconds = (uint) (totalSeconds / (ulong) _timeRemainingHistory.Count);
            var remainingTime = avgElapsedSeconds * (uint) (MaxBatches - CurrentBatch);
            var avgTimeSpan = TimeSpan.FromSeconds(remainingTime);
            result = $"\nTime Remaining: {avgTimeSpan}";
        }
        _lastBatchPassTime = DateTime.UtcNow;
        return result;
    }

    private void TerminateProcess()
    {
        _timeoutTimer.Dispose();
        _darknetProcess?.CancelOutputRead();
        _darknetProcess?.Kill();
        _darknetProcess?.Dispose();
        _consoleParser.BatchPassed -= OnBatchPassed;
        _consoleParser.Dispose();
    }

    private void SaveResult()
    {
        var configLastWeights = "Data/Trainer/Darknet/backup/config_last.weights";
        //var finalWeights = "Data/Trainer/Darknet/backup/config_final.weights";
        var lastWeights = "Data/Trainer/Darknet/backup/last.weights";
        
        if (File.Exists(configLastWeights)) File.Move(configLastWeights, lastWeights, true);
        if (File.Exists(lastWeights))
        {
            File.Move(lastWeights, ModelForTraining.WeightsPath, true);
            Log.Information("Moved latest weights from \"lastWeights\" to \"ModelForTraining.WeightsPath\"");
        }
        else throw new Exception($"File not found: {lastWeights}");
    }

    private void ClearFolder()
    {
        if (Directory.Exists(Path.Combine(DarknetFolder, "data", "train")))
        {
            foreach (var filePath in Directory.GetFiles(Path.Combine(DarknetFolder, "data", "train"))) File.Delete(filePath);
        }
        if (File.Exists(DarknetFolder + "/data/train.txt")) File.Delete(DarknetFolder + "/data/train.txt");
        if (Directory.Exists(DarknetFolder + "/backup"))
        {
            foreach (var filePath in Directory.GetFiles(DarknetFolder + "/backup"))
            {
                File.Delete(filePath);
            }
        }
    }
}
*/
