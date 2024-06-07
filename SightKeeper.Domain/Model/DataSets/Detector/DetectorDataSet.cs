namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet : DataSet
{
	public DetectorTagsLibrary Tags { get; }
	public DetectorAssetsLibrary Assets { get; }

	public DetectorDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new DetectorTagsLibrary(this);
		Assets = new DetectorAssetsLibrary();
		
	}
}