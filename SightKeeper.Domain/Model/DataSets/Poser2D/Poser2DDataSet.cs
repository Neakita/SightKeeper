namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DDataSet : DataSet<Poser2DTag, KeyPointTag2D, Poser2DAsset>
{
	public override Poser2DAssetsLibrary AssetsLibrary { get; }

	public Poser2DDataSet()
	{
		AssetsLibrary = new Poser2DAssetsLibrary(this);
	}
}