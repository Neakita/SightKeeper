using System.Globalization;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using Image = SixLabors.ImageSharp.Image;

namespace SightKeeper.Services;

public sealed class ImagesExporter
{
	private const string NumberFormat = "0.######";

	private static readonly NumberFormatInfo BoundingNumbersFormat = BoundingNumbersFormat = new NumberFormatInfo
	{
		NumberDecimalSeparator = "."
	};

	public ImagesExporter(ScreenshotImageLoader imageLoader, AssetsDataAccess assetsDataAccess)
	{
		_imageLoader = imageLoader;
		_assetsDataAccess = assetsDataAccess;
	}

	public Task<IReadOnlyCollection<string>> Export(
		string targetDirectoryPath,
		DataSet dataSet,
		CancellationToken cancellationToken = default) =>
		Export(targetDirectoryPath, dataSet.Assets, dataSet.ItemClasses, cancellationToken);

	public async Task<IReadOnlyCollection<string>> Export(
		string targetDirectoryPath,
		IReadOnlyCollection<Asset> assets,
		IReadOnlyCollection<ItemClass> itemClasses,
		CancellationToken cancellationToken = default) =>
		await Task.WhenAll(assets.Select(async (asset, index) =>
			await ExportAsync(targetDirectoryPath, index, asset, itemClasses, cancellationToken)));

	private readonly ScreenshotImageLoader _imageLoader;
	private readonly AssetsDataAccess _assetsDataAccess;

	private async Task<string> ExportAsync(string targetDirectoryPath, int index, Asset asset, IEnumerable<ItemClass> itemClasses, CancellationToken cancellationToken = default)
	{
		var imageFilePath = Path.Combine(targetDirectoryPath, $"image{index}.png");
		var infoFilePath = Path.Combine(targetDirectoryPath, $"image{index}.txt");
		_imageLoader.Load(asset.Screenshot);
		await Image.Load(asset.Screenshot.Image.Content).SaveAsync(imageFilePath, cancellationToken: cancellationToken);
		_assetsDataAccess.LoadItems(asset);
		await File.WriteAllTextAsync(infoFilePath, ItemsToString(asset.Items, itemClasses), cancellationToken);
		return imageFilePath;
	}

	private static string ItemsToString(IEnumerable<DetectorItem> items, IEnumerable<ItemClass> itemClasses)
	{
		var itemClassesIndexes = itemClasses
			.Select((itemClass, index) => (itemClass, index))
			.ToDictionary(x => x.itemClass, x => (byte) x.index);
		var stringItems = items.Select(item => ItemToString(item, itemClassesIndexes));
		var result = string.Join('\n', stringItems);
		return result;
	}

	private static string ItemToString(DetectorItem item, IReadOnlyDictionary<ItemClass, byte> itemClassesIndexes)
	{
		var itemClassIndex = itemClassesIndexes[item.ItemClass];
		return string.Join(' ', GetItemParameters(itemClassIndex, item.Bounding));
	}

	private static IEnumerable<string> GetItemParameters(byte itemClassIndex, Bounding bounding)
	{
		yield return itemClassIndex.ToString();
		yield return bounding.HorizontalCenter.ToString(NumberFormat, BoundingNumbersFormat);
		yield return bounding.VerticalCenter.ToString(NumberFormat, BoundingNumbersFormat);
		yield return bounding.Width.ToString(NumberFormat, BoundingNumbersFormat);
		yield return bounding.Height.ToString(NumberFormat, BoundingNumbersFormat);
	}
}