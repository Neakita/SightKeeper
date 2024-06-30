namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserDataSet : DataSet
{
	public override PoserTagsLibrary Tags { get; }
	public override PoserScreenshotsLibrary Screenshots { get; }
	public override PoserAssetsLibrary Assets { get; }
	public override PoserWeightsLibrary Weights { get; }

	public PoserDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new PoserTagsLibrary(this);
		Screenshots = new PoserScreenshotsLibrary(this);
		Assets = new PoserAssetsLibrary(this);
		Weights = new PoserWeightsLibrary(this);
	}
}