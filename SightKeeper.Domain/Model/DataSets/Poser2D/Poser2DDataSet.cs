namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DDataSet : DataSet
{
	public override Poser2DTagsLibrary Tags { get; }
	public override Poser2DScreenshotsLibrary Screenshots { get; }
	public override Poser2DAssetsLibrary Assets { get; }
	public override WeightsLibrary<Poser2DTag, KeyPointTag2D> Weights { get; }

	public Poser2DDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new Poser2DTagsLibrary(this);
		Screenshots = new Poser2DScreenshotsLibrary(this);
		Assets = new Poser2DAssetsLibrary(this);
		Weights = new WeightsLibrary<Poser2DTag, KeyPointTag2D>(this);
	}
}