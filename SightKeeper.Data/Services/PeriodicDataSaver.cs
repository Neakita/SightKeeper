using Serilog;

namespace SightKeeper.Data.Services;

public sealed class PeriodicDataSaver : ChangeListener, IDisposable
{
	public TimeSpan Period
	{
		get => _timer.Period;
		set => _timer.Period = value;
	}

	public PeriodicDataSaver(DataSaver dataSaver, ILogger logger)
	{
		_dataSaver = dataSaver;
		_logger = logger;
		Start();
	}

	public void SetDataChanged()
	{
		_dataChangedSinceLastSave = true;
	}

	public void Dispose()
	{
		if (_disposed)
			return;
		_timer.Dispose();
		_disposed = true;
	}

	private readonly DataSaver _dataSaver;
	private readonly ILogger _logger;
	private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));
	private bool _dataChangedSinceLastSave;
	private bool _disposed;

	private void Start()
	{
		var factory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
		factory.StartNew(SavePeriodically);
	}

	private async Task SavePeriodically()
	{
		while (await _timer.WaitForNextTickAsync())
			SaveIfNecessary();
	}

	private void SaveIfNecessary()
	{
		if (_dataChangedSinceLastSave)
			SaveData();
		_dataChangedSinceLastSave = false;
	}

	private void SaveData()
	{
		try
		{
			_dataSaver.Save();
		}
		catch (Exception exception)
		{
			_logger.Error(exception, "An exception was thrown when trying to save the data");
		}
	}
}