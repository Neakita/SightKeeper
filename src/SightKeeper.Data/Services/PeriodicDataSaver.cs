using Serilog;

namespace SightKeeper.Data.Services;

internal sealed class PeriodicDataSaver : ChangeListener, IDisposable
{
	public TimeSpan Period
	{
		get => _timer.Period;
		set => _timer.Period = value;
	}

	public PeriodicDataSaver(Lazy<DataSaver> dataSaver, ILogger logger)
	{
		_dataSaver = dataSaver;
		_logger = logger;
		logger.Verbose("Instantiated");
	}

	public void Start()
	{
		_processingTask = Task.Run(SavePeriodically);
		_logger.Verbose("Started");
	}

	public void SetDataChanged()
	{
		if (_processingTask == null)
			throw new InvalidOperationException($"{nameof(PeriodicDataSaver)} hasn't started yet but already receiving data change notification");
		if (_processingTask.IsCompleted)
			throw new InvalidOperationException($"{nameof(PeriodicDataSaver)} received data change notification but unable to process it as processing isn't running");
		_dataChangedSinceLastSave = true;
		_logger.Verbose("Data considered changed");
	}

	public void Dispose()
	{
		if (_disposed)
			return;
		_timer.Dispose();
		_disposed = true;
		_logger.Verbose("Disposed");
	}

	private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));
	private readonly Lazy<DataSaver> _dataSaver;
	private readonly ILogger _logger;
	private Task? _processingTask;
	private bool _dataChangedSinceLastSave;
	private bool _disposed;

	private async Task SavePeriodically()
	{
		while (await _timer.WaitForNextTickAsync())
			SaveIfNecessary();
	}

	private void SaveIfNecessary()
	{
		if (!_dataChangedSinceLastSave)
			return;
		SaveData();
		_dataChangedSinceLastSave = false;
		_logger.Verbose("Data saved");
	}

	private void SaveData()
	{
		try
		{
			_dataSaver.Value.Save();
		}
		catch (Exception exception)
		{
			_logger.Error(exception, "An exception was thrown when trying to save the data");
		}
	}
}