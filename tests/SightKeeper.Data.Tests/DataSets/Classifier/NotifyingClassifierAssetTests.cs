using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class NotifyingClassifierAssetTests
{
	[Fact]
	public void ShouldNotifyUsageChanged()
	{
		var asset = new NotifyingClassifierAsset(Substitute.For<ClassifierAsset>());
		using var monitor = asset.Monitor();
		asset.Usage = AssetUsage.Validation;
		monitor.Should().RaisePropertyChangeFor(classifierAsset => classifierAsset.Usage);
	}

	[Fact]
	public void ShouldNotifyTagChanged()
	{
		var asset = new NotifyingClassifierAsset(Substitute.For<ClassifierAsset>());
		using var monitor = asset.Monitor();
		asset.Tag = Substitute.For<Tag>();
		monitor.Should().RaisePropertyChangeFor(classifierAsset => classifierAsset.Tag);
	}
}