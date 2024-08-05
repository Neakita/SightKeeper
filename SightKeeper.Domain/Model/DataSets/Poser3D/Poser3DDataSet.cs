namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DDataSet : DataSet
{
	public override Poser3DTagsLibrary Tags { get; }
	public override Poser3DScreenshotsLibrary Screenshots { get; }
	public override Poser3DAssetsLibrary Assets { get; }
	public override Poser3DWeightsLibrary Weights { get; }

	public Poser3DDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new Poser3DTagsLibrary(this);
		Screenshots = new Poser3DScreenshotsLibrary(this);
		Assets = new Poser3DAssetsLibrary(this);
		Weights = new Poser3DWeightsLibrary(this);
	}
}