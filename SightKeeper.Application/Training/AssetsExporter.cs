﻿using System.Collections.Immutable;
using System.Globalization;
using Serilog;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Services;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;

namespace SightKeeper.Application.Training;

public sealed class AssetsExporter
{
	public AssetsExporter(ScreenshotsDataAccess screenshotsDataAccess, ILogger logger)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_logger = logger;
	}

	public void Export(
		string targetDirectoryPath,
		IReadOnlyCollection<DetectorAsset> assets,
		IReadOnlyCollection<Tag> tags)
	{
		throw new NotImplementedException();
		// var imagesDirectoryPath = Path.Combine(targetDirectoryPath, "images");
		// var labelsDirectoryPath = Path.Combine(targetDirectoryPath, "labels");
		// var data = _screenshotsDataAccess.LoadImages(assets.Select(asset => asset.Screenshot)).ToImmutableList();
		// ExportImages(imagesDirectoryPath, data.Select(d => d.image).ToImmutableList());
		// ExportLabels(labelsDirectoryPath, data.Select(d => _objectsLookupper.GetAsset(d.screenshot)).ToImmutableList(), tags);
	}
	
	private static readonly NumberFormatInfo NumberFormat = new() { NumberDecimalSeparator = "." };
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
	private readonly ILogger _logger;

	private void ExportImages(string directoryPath, IReadOnlyCollection<Domain.Model.DataSets.Screenshots.Image> images)
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

	private void ExportImage(string directoryPath, Domain.Model.DataSets.Screenshots.Image image, int assetIndex)
	{
		throw new NotImplementedException();
		// var imagePath = Path.Combine(directoryPath, $"{assetIndex}.png");
		// ExportImage(imagePath, image.Content);
	}

	private static void ExportImage(string path, byte[] content)
	{
		using MemoryStream stream = new(content);
		using var image = Image.Load(stream);
		image.Save(path);
	}

	private void ExportLabels(
		string directoryPath,
		ImmutableList<DetectorAsset> assets,
		IReadOnlyCollection<Tag> tags)
	{
		Directory.CreateDirectory(directoryPath);
		var operation = _logger.BeginOperation("Exporting labels for {AssetsCount} assets ({AssetsWithoutItems} without items) with {ItemsCount} items",
			assets.Count,
			assets.Count(asset => !asset.Items.Any()),
			assets.SelectMany(asset => asset.Items).Count());
		var tagsWithIndexes = tags
			.Select((tag, tagIndex) => (tag, tagIndex))
			.ToDictionary(tuple => tuple.tag, tuple => (byte)tuple.tagIndex);
		_logger.Debug("Item classes by indexes: {Tags}", tagsWithIndexes);
		var assetIndex = -1;
		foreach (var asset in assets)
			ExportLabels(directoryPath, asset, ++assetIndex, tagsWithIndexes);
		operation.Complete();
	}

	private void ExportLabels(string directoryPath, DetectorAsset asset, int assetIndex, Dictionary<Tag, byte> tags)
	{
		if (!asset.Items.Any())
			return;
		var labelPath = Path.Combine(directoryPath, $"{assetIndex}.txt");
		ExportLabels(labelPath, asset, tags);
	}

	private void ExportLabels(
		string path,
		DetectorAsset asset,
		Dictionary<Tag, byte> tags)
	{
		var content = string.Join('\n', asset.Items.Select(item => GetDetectorItemLabel(item, tags)));
		File.WriteAllText(path, content);
	}

	private static string GetDetectorItemLabel(DetectorItem item, Dictionary<Tag, byte> tags) =>
		string.Join(' ', GetDetectorItemLabelParameters(item, tags));

	private static IEnumerable<string> GetDetectorItemLabelParameters(
		DetectorItem item,
		Dictionary<Tag, byte> tags)
	{
		yield return tags[item.Tag].ToString();
		var bounding = item.Bounding;
		yield return bounding.Center.X.ToString(NumberFormat);
		yield return bounding.Center.Y.ToString(NumberFormat);
		yield return bounding.Size.X.ToString(NumberFormat);
		yield return bounding.Size.Y.ToString(NumberFormat);
	}
}