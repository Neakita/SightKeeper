using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests;

public sealed class ObservableImagesImageSetTests
{
	[Fact]
	public void ShouldObserveImageCreation()
	{
		ObservableImagesImageSet set = new(Substitute.For<ImageSet>());
		var observableList = (ReadOnlyObservableList<Image>)set.Images;
		List<IndexedChange<Image>> observedChanges = new();
		observableList.Subscribe(observedChanges.Add);
		var image = set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Insertion<Image>>().Subject;
		insertion.Index.Should().Be(0);
		insertion.Items.Should().ContainSingle().Which.Should().Be(image);
	}
}