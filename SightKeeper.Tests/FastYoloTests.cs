using System.Diagnostics;
using System.Drawing;
using FastYolo;
using FastYolo.Model;
using Xunit.Abstractions;

namespace SightKeeper.Tests;

public sealed class FastYoloTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	public FastYoloTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

	[Fact]
	public void ShouldRun()
	{
		const string yoloConfigFile = "Samples/yolov3-tiny.cfg";
		const string yoloWeightsFile = "Samples/yolov3-tiny.weights";
		const string yoloClassesFile = "Samples/coco.names";
		const string imageFilename = "Samples/cars road.jpg";
		using YoloWrapper yoloWrapper = new(yoloConfigFile, yoloWeightsFile, yoloClassesFile);

		Image image = Image.FromFile(imageFilename);
		using MemoryStream memoryStream = new();
		image.Save(memoryStream, image.RawFormat);
		byte[] byteArray = memoryStream.ToArray();

		IEnumerable<YoloItem> yoloItems = yoloWrapper.Detect(byteArray);

		foreach (YoloItem item in yoloItems)
			_testOutputHelper.WriteLine($"Object Found: {item.Type} with Shape: {item.Shape}, X: {item.X}, Y: {item.Y}, Width: {item.Width}, Height: {item.Height}");
	}

	[Fact]
	public void FPSBenchmark()
	{
		const string yoloConfigFile = "Samples/yolov3-tiny.cfg";
		const string yoloWeightsFile = "Samples/yolov3-tiny.weights";
		const string yoloClassesFile = "Samples/coco.names";
		const string imageFilename = "Samples/cars road.jpg";
		const int iterations = 1000;
		using YoloWrapper yoloWrapper = new(yoloConfigFile, yoloWeightsFile, yoloClassesFile);
		Image image = Image.FromFile(imageFilename);
		using MemoryStream memoryStream = new();
		image.Save(memoryStream, image.RawFormat);
		byte[] byteArray = memoryStream.ToArray();
		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < iterations; i++) yoloWrapper.Detect(byteArray);
		TimeSpan elapsed = stopwatch.Elapsed;
		stopwatch.Stop();
		_testOutputHelper.WriteLine($"Elapsed: {Math.Round(elapsed.TotalSeconds, 1)} seconds for {iterations} iterations.\nAverage fps: {Math.Round(iterations / elapsed.TotalSeconds, 1)}");
	}
}