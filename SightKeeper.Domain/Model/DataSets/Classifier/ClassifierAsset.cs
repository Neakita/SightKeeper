using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAsset : Asset
{
	public ClassifierTag Tag
	{
		get => _tag;
		set
		{
			_tag = value;
			Guard.IsTrue(DataSet.Tags.Contains(value));
		}
	}

	public ClassifierAssetsLibrary Library { get; }
	public ClassifierDataSet DataSet => Library.DataSet;

	// TODO store screenshot
	internal ClassifierAsset(Screenshot screenshot, ClassifierTag tag, ClassifierAssetsLibrary library)
	{
		_tag = tag;
		Library = library;
	}

	private ClassifierTag _tag;
}