using System.Text.Json;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Training.COCO;

internal sealed class COCODetectorDataSetExporter(ImageExporter imageExporter, ILogger logger) : TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>
{
	public async Task ExportAsync(string directoryPath, ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data, CancellationToken cancellationToken)
	{
		try
		{
			ProcessCategories(data.Tags);
			var assets = data.Assets.ToList();
			ProcessAssets(assets);
			var images = assets.Select(asset => asset.Image).ToList();
			ProcessImages(images);
			var imagesPath = Path.Combine(directoryPath, "images");
			await ExportImagesAsync(imagesPath, images, cancellationToken);
			var dataFilePath = Path.Combine(directoryPath, "data.json");
			await ExportDataAsync(dataFilePath, cancellationToken);
		}
		finally
		{
			Clear();
		}
	}

	private readonly Dictionary<ReadOnlyTag, int> _categoryIds = new();
	private readonly List<COCOCategory> _categories = new();
	private readonly List<COCOImage> _images = new();
	private readonly List<COCOAnnotation> _annotations = new();
	private int _annotationIdCounter;
	private COCODataSet DataSet => new()
	{
		Images = _images,
		Annotations = _annotations,
		Categories = _categories
	};

	private void ProcessCategories(IEnumerable<ReadOnlyTag> tags)
	{
		tags = tags.ToList();
		var categories = tags.Select(CreateCategory);
		_categories.AddRange(categories);
		foreach (var (index, tag) in tags.Index())
			_categoryIds.Add(tag, index);
	}

	private static COCOCategory CreateCategory(ReadOnlyTag tag, int id)
	{
		return new COCOCategory
		{
			Id = id,
			Name = tag.Name
		};
	}

	private void ProcessAssets(IEnumerable<ReadOnlyItemsAsset<ReadOnlyDetectorItem>> assets)
	{
		var annotations = assets.SelectMany(CreateAnnotations);
		_annotations.AddRange(annotations);
	}

	private IEnumerable<COCOAnnotation> CreateAnnotations(ReadOnlyItemsAsset<ReadOnlyDetectorItem> asset, int index)
	{
		return asset.Items
			.Select(asset.Image.Size, Denormalize)
			.Select(CreateAnnotation)
			.Do(index, SetImageId);
	}

	private static ReadOnlyDetectorItem Denormalize(ReadOnlyDetectorItem item, Vector2<ushort> imageSize)
	{
		return new DetectorItemValue
		{
			Tag = item.Tag,
			Bounding = item.Bounding * imageSize.ToDouble()
		};
	}

	private COCOAnnotation CreateAnnotation(ReadOnlyDetectorItem item)
	{
		var bounding = item.Bounding;
		return new COCOAnnotation
		{
			Id = _annotationIdCounter++,
			CategoryId = _categoryIds[item.Tag],
			Bbox =
			[
				bounding.Left,
				bounding.Top,
				bounding.Width,
				bounding.Height
			]
		};
	}

	private static void SetImageId(COCOAnnotation annotation, int id)
	{
		annotation.ImageId = id;
	}

	private void ProcessImages(IEnumerable<ImageData> images)
	{
		var cocoImages = images.Select(CreateCOCOImage);
		_images.AddRange(cocoImages);
	}

	private static COCOImage CreateCOCOImage(ImageData data, int id)
	{
		return new COCOImage
		{
			Id = id,
			Width = data.Size.X,
			Height = data.Size.Y,
			FileName = GetImageFileName(id),
			DateCaptured = data.CreationTimestamp.DateTime
		};
	}

	private async Task ExportImagesAsync(string directoryPath, IEnumerable<ImageData> images, CancellationToken cancellationToken)
	{
		foreach (var (index, image) in images.Index())
		{
			var fileName = GetImageFileName(index);
			var imageFilePath = Path.Combine(directoryPath, fileName);
			await imageExporter.ExportImageAsync(imageFilePath, image, cancellationToken);
		}
	}

	private static string GetImageFileName(int id)
	{
		return $"{id}.png";
	}

	private async Task ExportDataAsync(string filePath, CancellationToken cancellationToken)
	{
		using var operation = logger.OperationAt(LogEventLevel.Debug).Begin("Data export");
		await using var dataSetStream = File.Open(filePath, FileMode.Create);
		await JsonSerializer.SerializeAsync(dataSetStream, DataSet, COCODataSetSourceGenerationContext.Default.COCODataSet, cancellationToken);
		operation.Complete();
	}

	private void Clear()
	{
		_categoryIds.Clear();
		_categories.Clear();
		_images.Clear();
		_annotations.Clear();
		_annotationIdCounter = 0;
	}
}