using System.Diagnostics;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Serilog;
using Serilog.Events;

namespace SightKeeper.Application.Training;

public sealed class LinuxSessionCommandRunner : SessionCommandRunner
{
	public IObservable<string> Output => _output;

	public LinuxSessionCommandRunner() : this(Directory.GetCurrentDirectory())
	{
	}

	public LinuxSessionCommandRunner(string workingDirectory)
	{
		Directory.CreateDirectory(workingDirectory);
		_process = new Process();
		_process.OutputDataReceived += OnOutputReceived;
		_process.ErrorDataReceived += OnErrorReceived;
		_process.Exited += OnExited;
		var startInfo = new ProcessStartInfo
		{
			FileName = "/bin/bash",
			RedirectStandardInput = true,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			WorkingDirectory = workingDirectory,
			CreateNoWindow = true
		};
		_process.StartInfo = startInfo;
		bool isStarted = _process.Start();
		_process.BeginOutputReadLine();
		_process.BeginErrorReadLine();
		Guard.IsTrue(isStarted);
	}

	public void ExecuteCommand(string command)
	{
		_process.StandardInput.WriteLine(command);
		Logger.Verbose("Executing command: {command}", command);
	}

	public Task WaitAsync(CancellationToken cancellationToken)
	{
		return _process.WaitForExitAsync(cancellationToken);
	}

	public void Dispose()
	{
		ExecuteCommand("exit");
		_process.WaitForExit();
		_process.Dispose();
		_process.OutputDataReceived -= OnOutputReceived;
		_process.ErrorDataReceived -= OnErrorReceived;
		_output.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		ExecuteCommand("exit");
		await _process.WaitForExitAsync();
		_process.Dispose();
		_process.OutputDataReceived -= OnOutputReceived;
		_process.ErrorDataReceived -= OnErrorReceived;
		_output.Dispose();
	}

	private static readonly ILogger Logger = Log.ForContext<LinuxSessionCommandRunner>();
	private static readonly TimeSpan ExitTimeout = TimeSpan.FromSeconds(30);
	private readonly Process _process;
	private readonly Subject<string> _output = new();

	private void OnOutputReceived(object sender, DataReceivedEventArgs e)
	{
		if (e.Data == null)
			return;
		Logger.Verbose("{data}", e.Data);
		_output.OnNext(e.Data);
	}

	private void OnErrorReceived(object sender, DataReceivedEventArgs e)
	{
		if (e.Data != null)
			Logger.Error("{data}", e.Data);
	}

	private void OnExited(object? sender, EventArgs e)
	{
		_process.Exited -= OnExited;
		var logLevel = _process.ExitCode == 0 ? LogEventLevel.Debug : LogEventLevel.Warning;
		Logger.Write(logLevel, "Exited with code {exitCode}", _process.ExitCode);
	}
}