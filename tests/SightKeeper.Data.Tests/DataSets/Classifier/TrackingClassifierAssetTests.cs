using NSubstitute;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class TrackingClassifierAssetTests
{
	[Fact]
	public void ShouldNotifyChangeListenerWhenChangingUsage()
	{
		var innerAsset = Substitute.For<ClassifierAsset>();
		var changeListener = Substitute.For<ChangeListener>();
		var asset = new TrackingClassifierAsset(innerAsset, changeListener);
		asset.Usage = AssetUsage.Train;
		changeListener.Received().SetDataChanged();
	}

	[Fact]
	public void ShouldNotifyChangeListenerWhenChangingTag()
	{
		var innerAsset = Substitute.For<ClassifierAsset>();
		var changeListener = Substitute.For<ChangeListener>();
		var asset = new TrackingClassifierAsset(innerAsset, changeListener);
		asset.Tag = Substitute.For<Tag>();
		changeListener.Received().SetDataChanged();
	}
}