namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DDataSet : DataSet
{
	public override TagsLibrary<Poser3DTag> Tags { get; }
	public override AssetScreenshotsLibrary<Poser3DAsset> Screenshots { get; }
	public override AssetsLibrary<Poser3DAsset> Assets { get; }
	public override WeightsLibrary<Poser3DTag, KeyPointTag3D> Weights { get; }

	public Poser3DDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new TagsLibrary<Poser3DTag>(this);
		Screenshots = new AssetScreenshotsLibrary<Poser3DAsset>(this);
		Assets = new AssetsLibrary<Poser3DAsset>(this);
		Weights = new WeightsLibrary<Poser3DTag, KeyPointTag3D>(this);
	}
}