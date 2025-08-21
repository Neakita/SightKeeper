using System.Collections.ObjectModel;
using System.Text.Json;
using SightKeeper.Application.Training.Assets.Distribution;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Training.COCO;

public sealed class COCODetectorDataSetExporter(ImageExporter imageExporter) : DataSetTrainerExporter<DetectorDataSet>
{
	public string DirectoryPath { get; set; } = Directory.GetCurrentDirectory();

	public async Task ExportAsync(
		DetectorDataSet dataSet,
		AssetsDistributionRequest assetsDistributionRequest,
		CancellationToken cancellationToken)
	{
		Directory.CreateDirectory(DirectoryPath);
		_imageIds = dataSet.AssetsLibrary.Images.Index().ToDictionary(tuple => tuple.Item, tuple => tuple.Index + 1);
		_tagIds = dataSet.TagsLibrary.Tags.Index().ToDictionary(tuple => tuple.Item, tuple => tuple.Index + 1);
		try
		{
			var assetsDistribution = AssetsDistributor.DistributeAssets(dataSet.AssetsLibrary.Assets, assetsDistributionRequest);
			var tags = dataSet.TagsLibrary.Tags;
			await ExportGroupAsync("train", assetsDistribution.TrainAssets, tags, cancellationToken);
			await ExportGroupAsync("validation", assetsDistribution.ValidationAssets, tags, cancellationToken);
			await ExportGroupAsync("test", assetsDistribution.TestAssets, tags, cancellationToken);
		}
		finally
		{
			_imageIds = ReadOnlyDictionary<Image, int>.Empty;
			_tagIds = ReadOnlyDictionary<Tag, int>.Empty;
		}
	}

	private int _annotationIdCounter;
	private IReadOnlyDictionary<Image, int> _imageIds = ReadOnlyDictionary<Image, int>.Empty;
	private IReadOnlyDictionary<Tag, int> _tagIds = ReadOnlyDictionary<Tag, int>.Empty;

	private async Task ExportGroupAsync(string groupName, IReadOnlyList<ItemsAsset<DetectorItem>> assets, IReadOnlyList<Tag> tags, CancellationToken cancellationToken)
	{
		var cocoDataSet = CreateCOCODataSet(assets, tags);
		var cocoDataSetPath = Path.Combine(DirectoryPath, $"{groupName}.json");
		await WriteCOCODataSet(cocoDataSetPath, cocoDataSet, cancellationToken);
		var imagesPath = Path.Combine(DirectoryPath, groupName);
		await ExportImagesAsync(imagesPath, assets.Select(asset => asset.Image), cancellationToken);
	}

	private static async Task WriteCOCODataSet(string filePath, COCODataSet set, CancellationToken cancellationToken)
	{
		await using var stream = File.Open(filePath, FileMode.Create, FileAccess.Write);
		await JsonSerializer.SerializeAsync(stream, set, COCODataSetSourceGenerationContext.Default.COCODataSet, cancellationToken);
	}

	private COCODataSet CreateCOCODataSet(IReadOnlyList<ItemsAsset<DetectorItem>> assets, IReadOnlyList<Tag> tags)
	{
		var cocoDataSet = new COCODataSet
		{
			Images = GetCOCOImages(assets),
			Annotations = GetCOCOAnnotations(assets),
			Categories = GetCOCOCategories(tags)
		};
		return cocoDataSet;
	}

	private COCOImage[] GetCOCOImages(IReadOnlyList<Asset> assets)
	{
		var cocoImages = new COCOImage[assets.Count];
		for (var i = 0; i < assets.Count; i++)
		{
			var image = assets[i].Image;
			var imageId = _imageIds[image];
			cocoImages[i] = new COCOImage
			{
				Id = imageId,
				Width = image.Size.X,
				Height = image.Size.Y,
				FileName = $"{imageId}.png",
				DateCaptured = image.CreationTimestamp.DateTime
			};
		}
		return cocoImages;
	}

	private COCOAnnotation[] GetCOCOAnnotations(IReadOnlyCollection<ItemsAsset<DetectorItem>> assets)
	{
		var itemsCount = assets.Sum(asset => asset.Items.Count);
		var cocoAnnotations = new COCOAnnotation[itemsCount];
		var itemIndex = 0;
		foreach (var asset in assets)
		{
			var image = asset.Image;
			var imageId = _imageIds[image];
			var imageWidth = image.Size.X;
			var imageHeight = image.Size.Y;
			foreach (var item in asset.Items)
			{
				var tagId = _tagIds[item.Tag];
				var bounding = item.Bounding;
				cocoAnnotations[itemIndex] = new COCOAnnotation
				{
					Id = ++_annotationIdCounter,
					ImageId = imageId,
					CategoryId = tagId,
					Bbox = [bounding.Left * imageWidth, bounding.Top * imageHeight, bounding.Width * imageWidth, bounding.Height * imageHeight]
				};
				itemIndex++;
			}
		}
		return cocoAnnotations;
	}

	private COCOCategory[] GetCOCOCategories(IReadOnlyList<Tag> tags)
	{
		var cocoCategories = new COCOCategory[tags.Count];
		for (int i = 0; i < tags.Count; i++)
		{
			var tag = tags[i];
			cocoCategories[i] = new COCOCategory
			{
				Id = i + 1,
				Name = tag.Name
			};
		}
		return cocoCategories;
	}

	private async Task ExportImagesAsync(string directoryPath, IEnumerable<Image> images, CancellationToken cancellationToken)
	{
		Directory.CreateDirectory(directoryPath);
		foreach (var image in images)
			await ExportImageAsync(directoryPath, image, cancellationToken);
	}

	private Task ExportImageAsync(string directoryPath, Image image, CancellationToken cancellationToken)
	{
		var imageId = _imageIds[image];
		var fileName = $"{imageId}.png";
		var filePath = Path.Combine(directoryPath, fileName);
		return imageExporter.ExportImageAsync(filePath, image, cancellationToken);
	}
}