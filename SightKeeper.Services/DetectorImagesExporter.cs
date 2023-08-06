using System.Globalization;
using SightKeeper.Application.Annotating;
using SightKeeper.Application.Training.Images;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using Image = SixLabors.ImageSharp.Image;

namespace SightKeeper.Services;

public sealed class DetectorImagesExporter : ImagesExporter<DetectorModel>
{
	private const string NumberFormat = "0.######";

	private static readonly NumberFormatInfo BoundingNumbersFormat = BoundingNumbersFormat = new NumberFormatInfo
	{
		NumberDecimalSeparator = ".",
	};

	public DetectorImagesExporter(ScreenshotImageLoader imageLoader, DetectorAssetsDataAccess assetsDataAccess)
	{
		_imageLoader = imageLoader;
		_assetsDataAccess = assetsDataAccess;
	}
	
	public async Task<IReadOnlyCollection<string>> ExportAsync(string targetDirectoryPath, DetectorModel model,
		CancellationToken cancellationToken = default) =>
		await Task.WhenAll(model.Assets.Select(async (asset, index) =>
			await ExportAsync(targetDirectoryPath, index, asset, model.ItemClasses, cancellationToken)));
	
	private readonly ScreenshotImageLoader _imageLoader;
	private readonly DetectorAssetsDataAccess _assetsDataAccess;

	private async Task<string> ExportAsync(string targetDirectoryPath, int index, DetectorAsset asset, IEnumerable<ItemClass> itemClasses, CancellationToken cancellationToken = default)
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
		return string.Join(' ', GetItemParameters(itemClassIndex, item.BoundingBox));
	}

	private static IEnumerable<string> GetItemParameters(byte itemClassIndex, BoundingBox bounding)
	{
		yield return itemClassIndex.ToString();
		yield return bounding.XCenter.ToString(NumberFormat, BoundingNumbersFormat);
		yield return bounding.YCenter.ToString(NumberFormat, BoundingNumbersFormat);
		yield return bounding.Width.ToString(NumberFormat, BoundingNumbersFormat);
		yield return bounding.Height.ToString(NumberFormat, BoundingNumbersFormat);
	}
}