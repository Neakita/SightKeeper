namespace SightKeeper.Data.Tests.Services;

public sealed class AppDataClassifierAnnotatorTests
{
	[Fact]
	public void ShouldNotifyChangeListenerWhenSettingTag()
	{
		throw new NotImplementedException();
		/*DomainImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var changeListener = Substitute.For<ChangeListener>();
		AppDataClassifierAnnotator annotator = new(changeListener, new Lock(), new FakeAssetsMaker());
		annotator.SetTag(dataSet.AssetsLibrary, image, tag);
		changeListener.Received().SetDataChanged();*/
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		throw new NotImplementedException();
		/*DomainImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("1");
		dataSet.AssetsLibrary.MakeAsset(image);
		var changeListener = Substitute.For<ChangeListener>();
		AppDataClassifierAnnotator annotator = new(changeListener, new Lock(), new FakeAssetsMaker());
		annotator.DeleteAsset(dataSet.AssetsLibrary, image);
		changeListener.Received().SetDataChanged();*/
	}
}