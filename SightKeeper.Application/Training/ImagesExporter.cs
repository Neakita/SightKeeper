using System.Globalization;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using Image = SixLabors.ImageSharp.Image;

namespace SightKeeper.Application.Training;

public sealed class ImagesExporter
{
	public ImagesExporter(AssetsDataAccess assetsDataAccess, ScreenshotImageLoader imageLoader)
	{
		_assetsDataAccess = assetsDataAccess;
		_imageLoader = imageLoader;
	}
	
	public async Task Export(
		string targetDirectoryPath,
		Domain.Model.DataSet dataSet,
		CancellationToken cancellationToken = default)
	{
		await _assetsDataAccess.LoadAssets(dataSet, cancellationToken);
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
	private readonly AssetsDataAccess _assetsDataAccess;
	private readonly ScreenshotImageLoader _imageLoader;

	private Task ExportImages(string directoryPath, IReadOnlyCollection<Asset> assets, CancellationToken cancellationToken) =>
		Task.WhenAll(assets.Select((asset, assetIndex) => ExportImage(directoryPath, asset, assetIndex, cancellationToken)));

	private async Task ExportImage(string directoryPath, Asset asset, int assetIndex, CancellationToken cancellationToken)
	{
		var imagePath = Path.Combine(directoryPath, $"{assetIndex}.png");
		var image = await _imageLoader.LoadAsync(asset.Screenshot, cancellationToken);
		await ExportImage(imagePath, image.Content, cancellationToken);
	}

	private static async Task ExportImage(string path, byte[] content, CancellationToken cancellationToken)
	{
		using MemoryStream stream = new(content);
		using var image = await Image.LoadAsync(stream, cancellationToken);
		await image.SaveAsync(path, cancellationToken);
	}

	private Task ExportLabels(
		string directoryPath,
		IEnumerable<Asset> assets,
		IReadOnlyCollection<ItemClass> itemClasses,
		CancellationToken cancellationToken)
	{
		var itemClassesWithIndexes = itemClasses
			.Select((itemClass, itemClassIndex) => (itemClass, itemClassIndex))
			.ToDictionary(tuple => tuple.itemClass, tuple => (byte)tuple.itemClassIndex);
		return Task.WhenAll(assets.Select((asset, assetIndex) => ExportLabels(directoryPath, asset, assetIndex, itemClassesWithIndexes, cancellationToken)));
	}

	private Task ExportLabels(string directoryPath, Asset asset, int assetIndex, Dictionary<ItemClass, byte> itemClasses, CancellationToken cancellationToken)
	{
		var labelPath = Path.Combine(directoryPath, $"{assetIndex}.txt");
		return ExportLabels(labelPath, asset, itemClasses, cancellationToken);
	}

	private async Task ExportLabels(
		string path,
		Asset asset,
		Dictionary<ItemClass, byte> itemClasses,
		CancellationToken cancellationToken)
	{
		await _assetsDataAccess.LoadItems(asset, cancellationToken);
		if (!asset.Items.Any())
			return;
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
		yield return bounding.HorizontalCenter.ToString(NumberFormat);
		yield return bounding.VerticalCenter.ToString(NumberFormat);
		yield return bounding.Width.ToString(NumberFormat);
		yield return bounding.Height.ToString(NumberFormat);
	}
}