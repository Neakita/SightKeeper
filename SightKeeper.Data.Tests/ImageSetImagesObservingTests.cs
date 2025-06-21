using FluentAssertions;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests;

public sealed class ImageSetImagesObservingTests
{
	[Fact]
	public void ShouldObserveImageCreation()
	{
		var set = Utilities.CreateImageSet();
		var observableList = (ReadOnlyObservableList<Image>)set.Images;
		List<IndexedChange<Image>> observedChanges = new();
		observableList.Subscribe(observedChanges.Add);
		var image = set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Insertion<Image>>().Subject;
		insertion.Index.Should().Be(0);
		insertion.Items.Should().ContainSingle().Which.Should().Be(image);
	}
}