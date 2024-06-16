namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserDataSet : DataSet
{
	public PoserTagsLibrary Tags { get; }
	public PoserAssetsLibrary Assets { get; }
	public PoserWeightsLibrary Weights { get; }

	public PoserDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new PoserTagsLibrary(this);
		Assets = new PoserAssetsLibrary(this);
		Weights = new PoserWeightsLibrary(this);
	}
}