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
			EnsureClosed(process);
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

	private static void EnsureClosed(Process process)
	{
		try
		{
			if (process.HasExited)
				return;
			process.Kill();
		}
		catch (InvalidOperationException exception)
		{
			Log.Debug(exception, "Expected exception has occured");
		}
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