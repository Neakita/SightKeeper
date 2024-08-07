namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DDataSet : DataSet<Poser2DTag, KeyPointTag2D, Poser2DAsset>
{
	public Poser2DDataSet(string name, ushort resolution) : base(name, resolution)
	{
	}
}