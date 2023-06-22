using System.Diagnostics;
using Serilog;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Services.Tests;

public sealed class DarknetHelperTests
{
	/*[Fact]
	public void ShouldRunDarknet()
	{
		Log.Logger = new LoggerConfiguration().WriteTo.File("log.txt").CreateLogger();
		
		DetectorModel model = new("Test model");
		ItemClass itemClass = new("Item class");
		model.ItemClasses.Add(itemClass);
		byte[] imageBytes = File.ReadAllBytes("Samples/320screenshot.png");
		Image image = new(imageBytes);
		DetectorScreenshot screenshot = new(image);
		screenshot.Items.Add(new DetectorItem(itemClass, new BoundingBox(0, 0, 1, 1)));
		model.DetectorScreenshots.Add(screenshot);
		model.Config = new DetectorConfig("YoloV3", File.ReadAllText("Samples/YoloV3.config"));

		DarknetHelper helper = new(new DetectorImagesExporter());
		DarknetProcess process = helper.StartNewTrainer(model);
		process.OutputReceived.Subscribe(Console.WriteLine);
		Process? darknetProcess = null;
		while (darknetProcess == null)
		{
			darknetProcess = Process.GetProcessesByName("darknet.exe").FirstOrDefault();
		}
		darknetProcess.WaitForExit();
	}*/
}
