using NSubstitute;
using SightKeeper.Data.Images;

namespace SightKeeper.Data.Tests;

public sealed class ImageSetChangeListenerTests
{
	[Fact]
	public void ShouldNotifyChangeListener()
	{
		var changeListener = Substitute.For<ChangeListener>();
		StorableImageSetFactory factory = new(changeListener, new Lock());
		var set = factory.CreateImageSet();
		set.Name = "New name";
		changeListener.Received().SetDataChanged();
	}

	[Fact]
	public void ShouldNotNotifyChangeListenerAfterCreation()
	{
		var changeListener = Substitute.For<ChangeListener>();
		StorableImageSetFactory factory = new(changeListener, new Lock());
		factory.CreateImageSet();
		changeListener.DidNotReceive().SetDataChanged();
	}
}