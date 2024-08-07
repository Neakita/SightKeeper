namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DDataSet : DataSet
{
	public override Poser3DTagsLibrary Tags { get; }
	public override AssetScreenshotsLibrary<Poser3DAsset> Screenshots { get; }
	public override Poser3DAssetsLibrary Assets { get; }
	public override WeightsLibrary<Poser3DTag, KeyPointTag3D> Weights { get; }

	public Poser3DDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new Poser3DTagsLibrary(this);
		Screenshots = new AssetScreenshotsLibrary<Poser3DAsset>(this);
		Assets = new Poser3DAssetsLibrary(this);
		Weights = new WeightsLibrary<Poser3DTag, KeyPointTag3D>(this);
	}
}