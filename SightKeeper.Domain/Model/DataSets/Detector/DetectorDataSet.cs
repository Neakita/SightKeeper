namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet : DataSet<DetectorTag, DetectorAsset>
{
	public DetectorDataSet(string name, ushort resolution) : base(name, resolution)
	{
	}
}