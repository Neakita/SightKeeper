using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Serilog;
using SightKeeper.Application.Training.Data;

namespace SightKeeper.Application.Training;

public sealed class DarknetProcessImplementation : DarknetProcess, IDisposable
{
	public IObservable<string> OutputReceived => _outputReceived.AsObservable();

	public DarknetProcessImplementation(ILogger? logger = null)
	{
		_logger = logger.WithGlobal();
	}
	public async Task RunAsync(DarknetArguments arguments, CancellationToken cancellationToken = default)
	{
		var process = RunProcess(arguments);
		try
		{
			await process.WaitForExitAsync(cancellationToken);
		}
		catch (OperationCanceledException)
		{
			Log.Debug("Operation canceled");
		}
		finally
		{
			if (!process.HasExited) Close(process);
			Complete(process);
		}
	}

	private Process RunProcess(DarknetArguments arguments)
	{
		ProcessStartInfo processStartInfo = new(DarknetPaths.DarknetExecutablePath, arguments.ToString())
		{
			WorkingDirectory = DarknetPaths.DarknetDirectory,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = arguments.DoNotShow
		};
		Process process = new();
		process.StartInfo = processStartInfo;
		process.OutputDataReceived += OnOutputDataReceived;
		process.ErrorDataReceived += OnOutputDataReceived;
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();
		return process;
	}

	private void Complete(Process process)
	{
		process.OutputDataReceived -= OnOutputDataReceived;
		process.Dispose();
		_outputReceived.OnCompleted();
	}

	private void Close(Process process)
	{
		process.Close();
		if (process.WaitForExit(1000)) return;
		_logger.Warning("Process did not exit in time, terminating...");
		process.Kill();
		_logger.Information("Process terminated");
	}
	
	public void Dispose()
	{
		_outputReceived.Dispose();
	}
	
	private readonly Subject<string> _outputReceived = new();
	private readonly ILogger _logger;

	private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (e.Data != null)
		{
			_logger.Verbose("Received output from darknet: {Data}", e.Data);
			_outputReceived.OnNext(e.Data);
		}
		else _logger.Warning("Received null output from darknet");
	}
}