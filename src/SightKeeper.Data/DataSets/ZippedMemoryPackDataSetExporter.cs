using System.IO.Compression;
using MemoryPack;
using SightKeeper.Application.DataSets;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets;

public sealed class ZippedMemoryPackDataSetExporter : DataSetExporter<DataSet>
{
	public async Task ExportAsync(string archivePath, DataSet set, CancellationToken cancellationToken)
	{
		await using var stream = File.Open(archivePath, FileMode.Create); 
		using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
		await WriteSetDataAsync(archive, set, cancellationToken);
		var images = set.AssetsLibrary.Images;
		await WriteImagesMetadataAsync(archive, images, cancellationToken);
		await WriteImagesAsync(archive, images, cancellationToken);
	}

	private static async Task WriteSetDataAsync(ZipArchive archive, DataSet set, CancellationToken cancellationToken)
	{
		var entry = archive.CreateEntry("data.bin");
		await using var stream = entry.Open();
		await MemoryPackSerializer.SerializeAsync(stream, set, cancellationToken: cancellationToken);
	}

	private static async Task WriteImagesMetadataAsync(ZipArchive archive, IReadOnlyCollection<ManagedImage> images, CancellationToken cancellationToken)
	{
		var entry = archive.CreateEntry("images.bin");
		await using var stream = entry.Open();
		await MemoryPackSerializer.SerializeAsync(stream, images, cancellationToken: cancellationToken);
	}

	private static async Task WriteImagesAsync(ZipArchive archive, IEnumerable<ManagedImage> images, CancellationToken cancellationToken)
	{
		foreach (var image in images)
			await WriteImageAsync(archive, image, cancellationToken);
	}

	private static async Task WriteImageAsync(ZipArchive archive, ManagedImage image, CancellationToken cancellationToken)
	{
		await using var imageStream = image.OpenReadStream();
		if (imageStream == null)
			return;
		var path = Path.Combine("images", image.Id.ToString());
		if (image.DataFormat != null)
			path += $".{image.DataFormat}";
		var entry = archive.CreateEntry(path);
		await using var stream = entry.Open();
		await imageStream.CopyToAsync(stream, cancellationToken);
	}
}