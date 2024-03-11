using System.Globalization;
using Serilog;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Model.DataSets;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;

namespace SightKeeper.Application.Training;

public sealed class ImagesExporter
{
	public ImagesExporter(ScreenshotsDataAccess screenshotsDataAccess, ILogger logger)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_logger = logger;
	}
	
	public async Task Export(
		string targetDirectoryPath,
		DataSet dataSet,
		CancellationToken cancellationToken = default)
	{
		await Export(targetDirectoryPath, dataSet.Assets, dataSet.ItemClasses, cancellationToken);
	}

	public Task Export(
		string targetDirectoryPath,
		IReadOnlyCollection<Asset> assets,
		IReadOnlyCollection<ItemClass> itemClasses,
		CancellationToken cancellationToken = default)
	{
		var imagesDirectoryPath = Path.Combine(targetDirectoryPath, "images");
		var labelsDirectoryPath = Path.Combine(targetDirectoryPath, "labels");
		Directory.CreateDirectory(imagesDirectoryPath);
		Directory.CreateDirectory(labelsDirectoryPath);
		return Task.WhenAll(
			ExportImages(imagesDirectoryPath, assets, cancellationToken),
			ExportLabels(labelsDirectoryPath, assets, itemClasses, cancellationToken));
	}
	
	private static readonly NumberFormatInfo NumberFormat = new() { NumberDecimalSeparator = "." };
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
	private readonly ILogger _logger;

	private async Task ExportImages(string directoryPath, IReadOnlyCollection<Asset> assets, CancellationToken cancellationToken)
	{
		var operation = _logger.BeginOperation("Exporting images for {AssetsCount} assets", assets.Count);
		var screenshots = assets.Select(asset => asset.Screenshot);
		var images = _screenshotsDataAccess.LoadImages(screenshots);
		await Task.WhenAll(images.Select((image, imageIndex) =>
			ExportImage(directoryPath, image, imageIndex, cancellationToken)));
		operation.Complete();
	}

	private async Task ExportImage(string directoryPath, Domain.Model.DataSets.Image image, int assetIndex, CancellationToken cancellationToken)
	{
		var imagePath = Path.Combine(directoryPath, $"{assetIndex}.png");
		await ExportImage(imagePath, image.Content, cancellationToken);
	}

	private static Task ExportImage(string path, byte[] content, CancellationToken cancellationToken)
	{
		return Task.Run(() =>
		{
			using MemoryStream stream = new(content);
			using var image = Image.Load(stream);
			image.Save(path);
		}, cancellationToken);
	}

	private async Task ExportLabels(
		string directoryPath,
		IReadOnlyCollection<Asset> assets,
		IReadOnlyCollection<ItemClass> itemClasses,
		CancellationToken cancellationToken)
	{
		var operation = _logger.BeginOperation("Exporting labels for {AssetsCount} assets ({AssetsWithoutItems} without items) with {ItemsCount} items",
			assets.Count,
			assets.Count(asset => !asset.Items.Any()),
			assets.SelectMany(asset => asset.Items).Count());
		var itemClassesWithIndexes = itemClasses
			.Select((itemClass, itemClassIndex) => (itemClass, itemClassIndex))
			.ToDictionary(tuple => tuple.itemClass, tuple => (byte)tuple.itemClassIndex);
		_logger.Debug("Item classes by indexes: {ItemClasses}", itemClassesWithIndexes);
		await Task.WhenAll(assets.Select((asset, assetIndex) => ExportLabels(directoryPath, asset, assetIndex, itemClassesWithIndexes, cancellationToken)));
		operation.Complete();
	}

	private async Task ExportLabels(string directoryPath, Asset asset, int assetIndex, Dictionary<ItemClass, byte> itemClasses, CancellationToken cancellationToken)
	{
		if (!asset.Items.Any())
			return;
		var labelPath = Path.Combine(directoryPath, $"{assetIndex}.txt");
		await ExportLabels(labelPath, asset, itemClasses, cancellationToken);
	}

	private async Task ExportLabels(
		string path,
		Asset asset,
		Dictionary<ItemClass, byte> itemClasses,
		CancellationToken cancellationToken)
	{
		var content = string.Join('\n', asset.Items.Select(item => GetDetectorItemLabel(item, itemClasses)));
		await File.WriteAllTextAsync(path, content, cancellationToken);
	}

	private static string GetDetectorItemLabel(DetectorItem item, Dictionary<ItemClass, byte> itemClasses) =>
		string.Join(' ', GetDetectorItemLabelParameters(item, itemClasses));

	private static IEnumerable<string> GetDetectorItemLabelParameters(
		DetectorItem item,
		Dictionary<ItemClass, byte> itemClasses)
	{
		yield return itemClasses[item.ItemClass].ToString();
		var bounding = item.Bounding;
		yield return bounding.Center.X.ToString(NumberFormat);
		yield return bounding.Center.Y.ToString(NumberFormat);
		yield return bounding.Size.X.ToString(NumberFormat);
		yield return bounding.Size.Y.ToString(NumberFormat);
	}
}