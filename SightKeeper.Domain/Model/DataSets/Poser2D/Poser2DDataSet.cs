namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DDataSet : DataSet
{
	public override Poser2DTagsLibrary Tags { get; }
	public override AssetScreenshotsLibrary<Poser2DAsset> Screenshots { get; }
	public override Poser2DAssetsLibrary Assets { get; }
	public override WeightsLibrary<Poser2DTag, KeyPointTag2D> Weights { get; }

	public Poser2DDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new Poser2DTagsLibrary(this);
		Screenshots = new AssetScreenshotsLibrary<Poser2DAsset>(this);
		Assets = new Poser2DAssetsLibrary(this);
		Weights = new WeightsLibrary<Poser2DTag, KeyPointTag2D>(this);
	}
}