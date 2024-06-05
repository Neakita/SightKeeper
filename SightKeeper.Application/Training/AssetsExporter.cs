using System.Collections.Immutable;
using System.Globalization;
using Serilog;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;

namespace SightKeeper.Application.Training;

public sealed class AssetsExporter
{
	public AssetsExporter(ScreenshotsDataAccess screenshotsDataAccess, ILogger logger, ObjectsLookupper objectsLookupper)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_logger = logger;
		_objectsLookupper = objectsLookupper;
	}

	public void Export(
		string targetDirectoryPath,
		IReadOnlyCollection<Asset> assets,
		IReadOnlyCollection<ItemClass> itemClasses)
	{
		var imagesDirectoryPath = Path.Combine(targetDirectoryPath, "images");
		var labelsDirectoryPath = Path.Combine(targetDirectoryPath, "labels");
		var data = _screenshotsDataAccess.LoadImages(assets.Select(asset => asset.Screenshot)).ToImmutableList();
		ExportImages(imagesDirectoryPath, data.Select(d => d.image).ToImmutableList());
		ExportLabels(labelsDirectoryPath, data.Select(d => _objectsLookupper.GetAsset(d.screenshot)).ToImmutableList(), itemClasses);
	}
	
	private static readonly NumberFormatInfo NumberFormat = new() { NumberDecimalSeparator = "." };
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
	private readonly ILogger _logger;
	private readonly ObjectsLookupper _objectsLookupper;

	private void ExportImages(string directoryPath, IReadOnlyCollection<Domain.Model.DataSets.Image> images)
	{
		Directory.CreateDirectory(directoryPath);
		var operation = _logger.BeginOperation("Exporting images for {AssetsCount} assets", images.Count);
		var imageIndex = -1;
		foreach (var image in images)
		{
			ExportImage(directoryPath, image, ++imageIndex);
		}
		operation.Complete();
	}

	private void ExportImage(string directoryPath, Domain.Model.DataSets.Image image, int assetIndex)
	{
		var imagePath = Path.Combine(directoryPath, $"{assetIndex}.png");
		ExportImage(imagePath, image.Content);
	}

	private static void ExportImage(string path, byte[] content)
	{
		using MemoryStream stream = new(content);
		using var image = Image.Load(stream);
		image.Save(path);
	}

	private void ExportLabels(
		string directoryPath,
		ImmutableList<Asset> assets,
		IReadOnlyCollection<ItemClass> itemClasses)
	{
		Directory.CreateDirectory(directoryPath);
		var operation = _logger.BeginOperation("Exporting labels for {AssetsCount} assets ({AssetsWithoutItems} without items) with {ItemsCount} items",
			assets.Count,
			assets.Count(asset => !asset.Items.Any()),
			assets.SelectMany(asset => asset.Items).Count());
		var itemClassesWithIndexes = itemClasses
			.Select((itemClass, itemClassIndex) => (itemClass, itemClassIndex))
			.ToDictionary(tuple => tuple.itemClass, tuple => (byte)tuple.itemClassIndex);
		_logger.Debug("Item classes by indexes: {ItemClasses}", itemClassesWithIndexes);
		var assetIndex = -1;
		foreach (var asset in assets)
			ExportLabels(directoryPath, asset, ++assetIndex, itemClassesWithIndexes);
		operation.Complete();
	}

	private void ExportLabels(string directoryPath, Asset asset, int assetIndex, Dictionary<ItemClass, byte> itemClasses)
	{
		if (!asset.Items.Any())
			return;
		var labelPath = Path.Combine(directoryPath, $"{assetIndex}.txt");
		ExportLabels(labelPath, asset, itemClasses);
	}

	private void ExportLabels(
		string path,
		Asset asset,
		Dictionary<ItemClass, byte> itemClasses)
	{
		var content = string.Join('\n', asset.Items.Select(item => GetDetectorItemLabel(item, itemClasses)));
		File.WriteAllText(path, content);
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