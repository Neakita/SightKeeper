using Serilog;

namespace SightKeeper.Data.Services;

public sealed class PeriodicDataSaver(DataSaver dataSaver, ILogger logger) : ChangeListener, IDisposable
{
	public TimeSpan Period
	{
		get => _timer.Period;
		set => _timer.Period = value;
	}

	public void Start()
	{
		Task.Run(SavePeriodically);
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

	private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));
	private bool _dataChangedSinceLastSave;
	private bool _disposed;

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
			dataSaver.Save();
		}
		catch (Exception exception)
		{
			logger.Error(exception, "An exception was thrown when trying to save the data");
		}
	}
}