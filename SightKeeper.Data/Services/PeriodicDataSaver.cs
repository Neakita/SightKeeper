using CommunityToolkit.Diagnostics;

namespace SightKeeper.Data.Services;

public sealed class PeriodicDataSaver : ChangeListener, IDisposable
{
	public required DataSaver DataSaver { get; init; }

	public TimeSpan Period
	{
		get => _timer.Period;
		set => _timer.Period = value;
	}

	public void Start()
	{
		ObjectDisposedException.ThrowIf(_disposed, this);
		Guard.IsNull(_task);
		_task = Task.Run(SavePeriodically);
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
	private Task? _task;
	private bool _dataChangedSinceLastSave;
	private bool _disposed;

	private async Task SavePeriodically()
	{
		while (await _timer.WaitForNextTickAsync())
		{
			if (_dataChangedSinceLastSave)
				DataSaver.Save();
			_dataChangedSinceLastSave = false;
		}
	}
}