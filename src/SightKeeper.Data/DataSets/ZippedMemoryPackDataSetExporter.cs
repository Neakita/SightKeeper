using System.IO.Compression;
using SightKeeper.Application.DataSets;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets;

internal sealed class ZippedMemoryPackDataSetExporter(Serializer<DataSet<Tag, Asset>> dataSetSerializer, Serializer<IReadOnlyCollection<ManagedImage>> imagesSerializer) : DataSetExporter<DataSet<Tag, Asset>>
{
	public async Task ExportAsync(string archivePath, DataSet<Tag, Asset> set, CancellationToken cancellationToken)
	{
		await using var stream = File.Open(archivePath, FileMode.Create); 
		using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
		await WriteSetDataAsync(archive, set, cancellationToken);
		var images = set.AssetsLibrary.Images;
		await WriteImagesMetadataAsync(archive, images, cancellationToken);
		await WriteImagesAsync(archive, images, cancellationToken);
	}

	private async Task WriteSetDataAsync(ZipArchive archive, DataSet<Tag, Asset> set, CancellationToken cancellationToken)
	{
		var entry = archive.CreateEntry("data.bin");
		await using var stream = entry.Open();
		await dataSetSerializer.SerializeAsync(stream, set, cancellationToken);
	}

	private async Task WriteImagesMetadataAsync(ZipArchive archive, IReadOnlyCollection<ManagedImage> images, CancellationToken cancellationToken)
	{
		var entry = archive.CreateEntry("images.bin");
		await using var stream = entry.Open();
		await imagesSerializer.SerializeAsync(stream, images, cancellationToken);
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