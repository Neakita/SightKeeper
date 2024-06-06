namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserDataSet : DataSet<PoserTag, PoserAsset, PoserAssetsLibrary>
{
	public PoserTag CreateTag(string name, uint color)
	{
		PoserTag tag = new(name, color);
		AddTag(tag);
		return tag;
	}

	public PoserDataSet(string name, ushort resolution) : base(new PoserAssetsLibrary(), name, resolution)
	{
	}
}