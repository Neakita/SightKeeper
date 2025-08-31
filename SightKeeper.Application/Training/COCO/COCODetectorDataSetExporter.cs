using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.COCO;

public sealed class COCODetectorDataSetExporter : TrainDataExporter<ItemsAssetData<AssetItemData>>
{
	public int CategoriesInitialId { get; set; } = 0;
	public IdCounter ImageIdCounter { get; set; } = new();
	public IdCounter AnnotationIdCounter { get; set; } = new();
	public bool ResetIdCounters { get; set; } = true;

	public async Task ExportAsync(string directoryPath, TrainData<ItemsAssetData<AssetItemData>> data, CancellationToken cancellationToken)
	{
		ResetCountersIfNecessary();
		var imagesDirectoryPath = Path.Combine(directoryPath, "images");
		Directory.CreateDirectory(imagesDirectoryPath);
		ProcessCategories(data.Tags);
		var images = new List<COCOImage>();
		var annotations = new List<COCOAnnotation>();
		foreach (var asset in data.Assets)
		{
			var image = CreateCOCOImage(asset.Image);
			images.Add(image);
			var imageFilePath = Path.Combine(imagesDirectoryPath, image.FileName);
			await ExportImage(imageFilePath, asset.Image, cancellationToken);
			foreach (var item in asset.Items)
			{
				var annotation = CreateAnnotation(item, image);
				annotations.Add(annotation);
			}
		}
		var dataSet = new COCODataSet
		{
			Images = images,
			Annotations = annotations,
			Categories = _categories
		};
		var dataSetFilePath = Path.Combine(directoryPath, "data.json");
		await using var dataSetStream = File.Open(dataSetFilePath, FileMode.Create);
		await JsonSerializer.SerializeAsync(dataSetStream, dataSet, COCODataSetSourceGenerationContext.Default.COCODataSet, cancellationToken);
	}

	private IReadOnlyList<COCOCategory> _categories = ReadOnlyCollection<COCOCategory>.Empty;
	private IReadOnlyDictionary<ReadOnlyTag, int> _categoryIds = ReadOnlyDictionary<ReadOnlyTag, int>.Empty;

	private void ResetCountersIfNecessary()
	{
		if (!ResetIdCounters)
			return;
		ImageIdCounter.Reset();
		AnnotationIdCounter.Reset();
	}

	private void ProcessCategories(IEnumerable<ReadOnlyTag> tags)
	{
		var idCounter = CategoriesInitialId;
		var categories = new List<COCOCategory>();
		var categoryIdLookup = new Dictionary<ReadOnlyTag, int>();
		foreach (var tag in tags)
		{
			var categoryId = idCounter++;
			var category = new COCOCategory
			{
				Id = categoryId,
				Name = tag.Name
			};
			categories.Add(category);
			categoryIdLookup.Add(tag, categoryId);
		}
		_categories = categories;
		_categoryIds = categoryIdLookup;
	}

	private COCOImage CreateCOCOImage(ImageData data)
	{
		var imageId = ImageIdCounter.NextId;
		var imageFileName = $"{imageId}.png";
		return new COCOImage
		{
			Id = imageId,
			Width = data.Size.X,
			Height = data.Size.Y,
			FileName = imageFileName,
			DateCaptured = data.CreationTimestamp.DateTime
		};
	}

	private COCOAnnotation CreateAnnotation(AssetItemData item, COCOImage image)
	{
		var bounding = item.Bounding;
		return new COCOAnnotation
		{
			Id = AnnotationIdCounter.NextId,
			ImageId = image.Id,
			CategoryId = _categoryIds[item.Tag],
			Bbox =
			[
				bounding.Left * image.Width,
				bounding.Top * image.Height,
				bounding.Width * image.Width,
				bounding.Height * image.Height
			]
		};
	}

	private static async Task ExportImage(string filePath, ImageData data, CancellationToken cancellationToken)
	{
		using var image = await data.LoadAsync(cancellationToken);
		Guard.IsNotNull(image);
		await image.SaveAsync(filePath, cancellationToken);
	}
}