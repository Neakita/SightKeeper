namespace SightKeeper.Data.Model.DataSets.Weights;

public readonly struct KeyPointTagLocation
{
	public readonly byte PoserTagIndex;
	public readonly byte KeyPointTagIndex;

	public KeyPointTagLocation(byte poserTagIndex, byte keyPointTagIndex)
	{
		PoserTagIndex = poserTagIndex;
		KeyPointTagIndex = keyPointTagIndex;
	}
}