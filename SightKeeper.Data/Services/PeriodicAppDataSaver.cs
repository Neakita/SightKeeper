namespace SightKeeper.Data.Services;

public sealed class PeriodicAppDataSaver : ChangeListener, IDisposable
{
	public PeriodicAppDataSaver(AppDataAccess appDataAccess)
	{
		_appDataAccess = appDataAccess;
	}

	public void Start()
	{
		new TaskFactory().StartNew(SavePeriodically, TaskCreationOptions.LongRunning);
	}

	public void SetDataChanged()
	{
		_dataChangedSinceLastSave = true;
	}

	public void Dispose()
	{
		_timer.Dispose();
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));

	private async Task SavePeriodically()
	{
		while (await _timer.WaitForNextTickAsync())
		{
			if (_dataChangedSinceLastSave)
				_appDataAccess.Save();
			_dataChangedSinceLastSave = false;
		}
	}

	private bool _dataChangedSinceLastSave;
}