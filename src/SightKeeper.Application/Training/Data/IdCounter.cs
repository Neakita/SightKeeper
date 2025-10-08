namespace SightKeeper.Application.Training.Data;

internal sealed class IdCounter(int initialId = 0)
{
	public int NextId => _id++;

	public void Reset()
	{
		_id = _initialId;
	}

	private readonly int _initialId = initialId;
	private int _id = initialId;
}