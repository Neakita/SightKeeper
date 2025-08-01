using NSubstitute;
using SightKeeper.Data.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class TrackableTagTests
{
	[Fact]
	public void ShouldCallChangeListenerWhenChangingName()
	{
		var innerTag = Substitute.For<StorableTag>();
		var changeListener = Substitute.For<ChangeListener>();
		var tag = new TrackableTag(innerTag, changeListener);
		tag.Name = "new name";
		changeListener.Received().SetDataChanged();
	}

	[Fact]
	public void ShouldCallChangeListenerWhenChangingColor()
	{
		var innerTag = Substitute.For<StorableTag>();
		var changeListener = Substitute.For<ChangeListener>();
		var tag = new TrackableTag(innerTag, changeListener);
		tag.Color = 123;
		changeListener.Received().SetDataChanged();
	}
}