using FluentAssertions;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Saving.DataSets;

public sealed class AssetSavingTests
{
	[Fact]
	public void ShouldPersistImage()
	{
		var asset = CreateAsset(out var dataSet, out var imageSet);
		var persistedAsset = Persist(dataSet, imageSet);
		persistedAsset.Image.CreationTimestamp.Should().Be(asset.Image.CreationTimestamp);
	}

	private static Asset CreateAsset(out DataSet dataSet, out DomainImageSet imageSet)
	{
		throw new NotImplementedException();
		/*dataSet = new DetectorDataSet();
		imageSet = new DomainImageSet();
		var image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return dataSet.AssetsLibrary.MakeAsset(image);*/
	}

	private static Asset Persist(DataSet dataSet, DomainImageSet imageSet)
	{
		throw new NotImplementedException();
		/*Guard.IsNotNull(dataSet);
		var assetsLibrary = dataSet.AssetsLibrary;
		Guard.IsEqualTo(assetsLibrary.Assets.Count, 1);
		AppDataAccess appDataAccess = new();
		Utilities.AddImageSetToAppData(imageSet, appDataAccess);
		Utilities.AddDataSetToAppData(dataSet, appDataAccess);
		var persistedAppData = appDataAccess.Data.Persist();
		var persistedDataSet = persistedAppData.DataSets.Single();
		return persistedDataSet.AssetsLibrary.Assets.Single();*/
	}
}