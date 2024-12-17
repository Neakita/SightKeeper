namespace SightKeeper.Data.Services;

public sealed class PeriodicAppDataSaver : IDisposable
{
	public PeriodicAppDataSaver(AppDataAccess appDataAccess)
	{
		_appDataAccess = appDataAccess;
	}

	public void Start()
	{
		new TaskFactory().StartNew(SavePeriodically, TaskCreationOptions.LongRunning);
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
			_appDataAccess.Save();
		_appDataAccess.Save();
	}
}