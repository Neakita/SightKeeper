using SightKeeper.Application.Training;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using Image = SixLabors.ImageSharp.Image;

namespace SightKeeper.Services.Training.Darknet;

public sealed class DetectorImagesExporter : ImagesExporter<DetectorModel>
{
	public IReadOnlyCollection<string> Export(string targetDirectoryPath, DetectorModel model) => 
		model.DetectorScreenshots.Select((screenshot, index) => Export(targetDirectoryPath, index, screenshot, model.ItemClasses)).ToList();
	
	private static string Export(string targetDirectoryPath, int index, DetectorScreenshot screenshot, IReadOnlyCollection<ItemClass> itemClasses)
	{
		string imageFilePath = Path.Combine(targetDirectoryPath, $"image{index}.png");
		string infoFilePath = Path.Combine(targetDirectoryPath, $"image{index}.txt");
		Image.Load(screenshot.Image.Content).Save(imageFilePath);
		File.WriteAllText(infoFilePath, ItemsToString(screenshot.Items, itemClasses));
		return imageFilePath;
	}

	private static string ItemsToString(IReadOnlyCollection<DetectorItem> items, IReadOnlyCollection<ItemClass> itemClasses)
	{
		Dictionary<ItemClass, byte> itemClassesIndexes = itemClasses
			.Select((itemClass, index) => (itemClass, index))
			.ToDictionary(x => x.itemClass, x => (byte) x.index);
		IEnumerable<string> stringItems = items.Select(item => ItemToString(item, itemClassesIndexes));
		string result = string.Join('\n', stringItems);
		return result;
	}

	private static string ItemToString(DetectorItem item, Dictionary<ItemClass, byte> itemClassesIndexes)
	{
		byte itemClassIndex = itemClassesIndexes[item.ItemClass];
		return $"{itemClassIndex} {item.BoundingBox.XCenter} {item.BoundingBox.YCenter} {item.BoundingBox.Width} {item.BoundingBox.Height}";
	}
}
