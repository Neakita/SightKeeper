namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet : DataSet<Tag, DetectorAsset, DetectorAssetsLibrary>
{
	public Tag CreateTag(string name, uint color)
	{
		Tag tag = new(name, color);
		AddTag(tag);
		return tag;
	}

	public DetectorDataSet(string name, ushort resolution) : base(new DetectorAssetsLibrary(), name, resolution)
	{
	}
}