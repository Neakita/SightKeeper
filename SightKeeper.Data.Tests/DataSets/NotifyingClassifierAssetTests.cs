using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class NotifyingClassifierAssetTests
{
	[Fact]
	public void ShouldNotifyUsageChanged()
	{
		var asset = new NotifyingClassifierAsset(Substitute.For<StorableClassifierAsset>());
		using var monitor = asset.Monitor();
		asset.Usage = AssetUsage.Validation;
		monitor.Should().RaisePropertyChangeFor(classifierAsset => classifierAsset.Usage);
	}

	[Fact]
	public void ShouldNotifyTagChanged()
	{
		var asset = new NotifyingClassifierAsset(Substitute.For<StorableClassifierAsset>());
		using var monitor = asset.Monitor();
		asset.Tag = Substitute.For<StorableTag>();
		monitor.Should().RaisePropertyChangeFor(classifierAsset => classifierAsset.Tag);
	}
}