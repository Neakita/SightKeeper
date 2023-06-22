using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Serilog;

namespace SightKeeper.Application.Training;

public sealed class DarknetProcess : IDisposable
{
	public IObservable<string> OutputReceived => _outputReceived.AsObservable();

	public bool IsRunning => Process != null;

	public DarknetProcess(string darknetExecutablePath, ILogger? logger = null)
	{
		_darknetExecutablePath = darknetExecutablePath;
		_logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Logger(Log.Logger).WriteTo.Logger(logger ?? new LoggerConfiguration().CreateLogger()).CreateLogger();
	}
	
	public void Run(string arguments)
	{
		ProcessStartInfo processStartInfo = new(_darknetExecutablePath, arguments)
		{
			WorkingDirectory = DarknetHelper.DarknetDirectory,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false
		};
		Process = new Process();
		Process.StartInfo = processStartInfo;
		Process.OutputDataReceived += OnOutputDataReceived;
		Process.ErrorDataReceived += OnOutputDataReceived;
		Process.Exited += OnProcessExited;
		Process.Start();
		Process.BeginOutputReadLine();
		Process.BeginErrorReadLine();
	}

	public void Run(DarknetArguments arguments) =>
		Run(arguments.ToString());

	public void Stop()
	{
		if (Process == null) throw new NullReferenceException("Process is null");
		Process.Exited -= OnProcessExited;
		if (!Process.HasExited)
		{
			Process.Close();
			if (!Process.WaitForExit(1000))
			{
				_logger.Warning("Process did not exit in time, terminating...");
				Process.Kill();
				_logger.Information("Process terminated");
			}
		}
		Process.OutputDataReceived -= OnOutputDataReceived;
		Process.Dispose();
		_outputReceived.OnCompleted();
	}
	
	public void Dispose()
	{
		if (Process != null) Stop();
		_outputReceived.Dispose();
	}
	
	private readonly Subject<string> _outputReceived = new();
	private readonly string _darknetExecutablePath;
	private readonly ILogger _logger;
	public Process? Process { get; private set; }

	private void NotifyOutputReceived(string output) => _outputReceived.OnNext(output);

	private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (e.Data != null)
		{
			_logger.Verbose("Received output from darknet: {Data}", e.Data);
			NotifyOutputReceived(e.Data);
		}
		else _logger.Warning("Received null output from darknet");
	}

	private void OnProcessExited(object? sender, EventArgs e)
	{
		_logger.Information("Process exited");
		Stop();
	}
}