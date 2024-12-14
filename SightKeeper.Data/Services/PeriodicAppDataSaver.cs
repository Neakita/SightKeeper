namespace SightKeeper.Data.Binary.Services;

public sealed class PeriodicAppDataSaver : IDisposable
{
	public TimeSpan Period
	{
		get => _timer.Period;
		set => _timer.Period = value;
	}

	public PeriodicAppDataSaver(AppDataAccess appDataAccess)
	{
		_appDataAccess = appDataAccess;
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