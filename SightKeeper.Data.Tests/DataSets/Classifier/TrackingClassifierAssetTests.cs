using NSubstitute;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class TrackingClassifierAssetTests
{
	[Fact]
	public void ShouldNotifyChangeListenerWhenChangingUsage()
	{
		var innerAsset = Substitute.For<StorableClassifierAsset>();
		var changeListener = Substitute.For<ChangeListener>();
		var asset = new TrackingClassifierAsset(innerAsset, changeListener);
		asset.Usage = AssetUsage.Train;
		changeListener.Received().SetDataChanged();
	}

	[Fact]
	public void ShouldNotifyChangeListenerWhenChangingTag()
	{
		var innerAsset = Substitute.For<StorableClassifierAsset>();
		var changeListener = Substitute.For<ChangeListener>();
		var asset = new TrackingClassifierAsset(innerAsset, changeListener);
		asset.Tag = Substitute.For<StorableTag>();
		changeListener.Received().SetDataChanged();
	}
}