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
	public async Task ExportAsync(
		string directoryPath,
		ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data,
		CancellationToken cancellationToken)
	{
		try
		{
			ProcessCategories(data.Tags);
			data = MaterializeData(data);
			ProcessAssets(data.Assets);
			var images = MaterializeImages(data);
			ProcessImages(images);
			await ExportImagesAsync(cancellationToken, directoryPath, images);
			await ExportDataAsync(directoryPath, cancellationToken);
		}
		finally
		{
			Clear();
		}
	}

	private async Task ExportImagesAsync(CancellationToken cancellationToken, string directoryPath, List<ImageData> images)
	{
		logger.Information("Exporting images");
		var imagesPath = Path.Combine(directoryPath, "images");
		await imageExporter.ExportImagesAsync(imagesPath, images, cancellationToken);
	}

	private List<ImageData> MaterializeImages(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data)
	{
		using var operation = logger.OperationAt(LogEventLevel.Debug).Begin("Images materialization");
		var images = data.Assets.Select(asset => asset.Image).ToList();
		operation.Complete();
		return images;
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
		logger.Information("Processing categories");
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

	private ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> MaterializeData(
		ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data)
	{
		using var operation = logger.OperationAt(LogEventLevel.Debug).Begin("Data meterialization");
		data = new ReadOnlyDataSetValue<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>(
			data.Tags.ToList(),
			data.Assets.ToList());
		operation.Complete();
		return data;
	}

	private void ProcessAssets(IEnumerable<ReadOnlyItemsAsset<ReadOnlyDetectorItem>> assets)
	{
		logger.Information("Processing assets");
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
		logger.Information("Processing images");
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

	private static string GetImageFileName(int id)
	{
		return $"{id}.png";
	}

	private async Task ExportDataAsync(string directoryPath, CancellationToken cancellationToken)
	{
		var filePath = Path.Combine(directoryPath, "data.json");
		logger.Information("Exporting data");
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