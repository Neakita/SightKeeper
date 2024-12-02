namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DDataSet : DataSet<Poser3DTag, KeyPointTag3D, Poser3DAsset>
{
	public override Poser3DAssetsLibrary AssetsLibrary { get; }

	public Poser3DDataSet()
	{
		AssetsLibrary = new Poser3DAssetsLibrary(this);
	}
}