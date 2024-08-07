namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierDataSet : DataSet<ClassifierTag, ClassifierAsset>
{
	public ClassifierDataSet(string name, ushort resolution) : base(name, resolution)
	{
	}
}