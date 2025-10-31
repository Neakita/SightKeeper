using System.Collections.ObjectModel;
using System.Text.Json;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Training.COCO;

internal sealed class COCODetectorDataSetExporter(ImageExporter imageExporter, ILogger logger) : TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>
{
	public async Task ExportAsync(string directoryPath, ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data, CancellationToken cancellationToken)
	{
		Reset();
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
			await imageExporter.ExportImageAsync(imageFilePath, asset.Image, cancellationToken);
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
		await ExportData(dataSetFilePath, dataSet, cancellationToken);
	}

	private IReadOnlyList<COCOCategory> _categories = ReadOnlyCollection<COCOCategory>.Empty;
	private IReadOnlyDictionary<ReadOnlyTag, int> _categoryIds = ReadOnlyDictionary<ReadOnlyTag, int>.Empty;
	private int _imageIdCounter;
	private int _annotationIdCounter;

	private void Reset()
	{
		_imageIdCounter = 0;
		_annotationIdCounter = 0;
	}

	private void ProcessCategories(IEnumerable<ReadOnlyTag> tags)
	{
		var idCounter = 0;
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
		var imageId = _imageIdCounter++;
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

	private COCOAnnotation CreateAnnotation(ReadOnlyDetectorItem item, COCOImage image)
	{
		var bounding = item.Bounding;
		return new COCOAnnotation
		{
			Id = _annotationIdCounter++,
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

	private async Task ExportData(string dataSetFilePath, COCODataSet dataSet, CancellationToken cancellationToken)
	{
		using var operation = logger.OperationAt(LogEventLevel.Debug).Begin("Data export");
		await using var dataSetStream = File.Open(dataSetFilePath, FileMode.Create);
		await JsonSerializer.SerializeAsync(dataSetStream, dataSet, COCODataSetSourceGenerationContext.Default.COCODataSet, cancellationToken);
		operation.Complete();
	}
}