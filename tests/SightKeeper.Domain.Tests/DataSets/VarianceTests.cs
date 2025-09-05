using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using Xunit.Abstractions;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class VarianceTests(ITestOutputHelper testOutputHelper)
{
	[Fact]
	public void Foo()
	{
		var classifierSet = new InMemoryDataSet<Tag, ClassifierAsset>();
		var detectorSet = new InMemoryDataSet<Tag, ItemsAsset<DetectorItem>>();
		var poserSet = new InMemoryDataSet<PoserTag, ItemsAsset<PoserItem>>();
		DataSet<Tag, Asset> dataSet = classifierSet;

		if (dataSet is DataSet<Tag, ClassifierAsset>)
		{
			testOutputHelper.WriteLine("Ok!");
		}
		else
		{
			throw new Exception();
		}
	}
}

public interface DataSet<out TTag, out TAsset>
{
	string Name { get; set; }
	string Description { get; set; }

	TagsOwner<TTag> TagsLibrary { get; }
	AssetsOwner<TAsset> AssetsLibrary { get; }
	WeightsLibrary WeightsLibrary { get; }
}

public sealed class InMemoryDataSet<TTag, TAsset> : DataSet<TTag, TAsset>
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public TagsOwner<TTag> TagsLibrary { get; }
	public AssetsOwner<TAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }
}