using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.ImageSets;

public sealed class ImageSetEditorTests
{
	[Fact]
	public void ShouldChangeImageSetProperties()
	{
		var editor = CreateEditor();
		var set = CreateImageSet();
		const string newName = "new name";
		const string newDescription = "new description";
		var data = Utilities.CreateExistingImageSetData(set, newName, newDescription);
		editor.EditImageSet(data);
		set.Name.Should().Be(newName);
		set.Description.Should().Be(newDescription);
	}

	[Fact]
	public void ShouldNotifyObserver()
	{
		var editor = CreateEditor();
		var observer = Substitute.For<IObserver<DomainImageSet>>();
		editor.Edited.Subscribe(observer);
		var set = CreateImageSet();
		var data = Utilities.CreateExistingImageSetData(set, "new name", "new description");
		editor.EditImageSet(data);
		observer.Received().OnNext(set);
	}

	private static ImageSetEditor CreateEditor()
	{
		return new ImageSetEditor();
	}

	private static DomainImageSet CreateImageSet()
	{
		return new DomainImageSet();
	}
}