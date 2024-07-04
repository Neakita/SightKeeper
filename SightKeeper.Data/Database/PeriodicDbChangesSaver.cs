using System.Timers;
using Microsoft.EntityFrameworkCore;
using Timer = System.Timers.Timer;

namespace SightKeeper.Data.Database;

public sealed class PeriodicDbChangesSaver : IDisposable
{
	public PeriodicDbChangesSaver(DbContext dbContext, TimeSpan interval)
	{
		_dbContext = dbContext;
		_timer = new Timer(interval);
		_timer.Elapsed += OnTimerElapsed;
	}

	private readonly Timer _timer;
	private readonly DbContext _dbContext;

	private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
	{
		_dbContext.SaveChanges();
	}

	public void Dispose()
	{
		_timer.Dispose();
	}
}