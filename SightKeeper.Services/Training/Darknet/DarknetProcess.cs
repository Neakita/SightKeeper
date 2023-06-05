using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Serilog;

namespace SightKeeper.Services.Training.Darknet;

public sealed class DarknetProcess : IDisposable
{
	public IObservable<string> OutputReceived => _outputReceived.AsObservable();

	public bool IsRunning => _process != null;

	public DarknetProcess(string darknetExecutablePath, ILogger? logger = null)
	{
		_darknetExecutablePath = darknetExecutablePath;
		_logger = new LoggerConfiguration().WriteTo.Logger(Log.Logger).WriteTo.Logger(logger ?? new LoggerConfiguration().CreateLogger()).CreateLogger();
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
		_process = new Process();
		_process.StartInfo = processStartInfo;
		_process.OutputDataReceived += OnOutputDataReceived;
		_process.ErrorDataReceived += OnOutputDataReceived;
		_process.Exited += OnProcessExited;
		_process.Start();
		_process.BeginOutputReadLine();
		_process.BeginErrorReadLine();
	}

	public void Run(DarknetArguments arguments) =>
		Run(arguments.ToString());

	public void Stop()
	{
		if (_process == null) throw new NullReferenceException("Process is null");
		_process.Exited -= OnProcessExited;
		if (!_process.HasExited)
		{
			_process.Close();
			if (!_process.WaitForExit(1000))
			{
				_logger.Warning("Process did not exit in time, terminating...");
				_process.Kill();
				_logger.Information("Process terminated");
			}
		}
		_process.OutputDataReceived -= OnOutputDataReceived;
		_process.Dispose();
		_outputReceived.OnCompleted();
	}
	
	public void Dispose()
	{
		if (_process != null) Stop();
		_outputReceived.Dispose();
	}
	
	private readonly Subject<string> _outputReceived = new();
	private readonly string _darknetExecutablePath;
	private readonly ILogger _logger;
	private Process? _process;

	private void NotifyOutputReceived(string output)
	{
		_logger.Verbose("Received output from darknet: {Output}", output);
		_outputReceived.OnNext(output);
	}

	private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (e.Data != null)
		{
			_logger.Debug("Received output from darknet: {Data}", e.Data);
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