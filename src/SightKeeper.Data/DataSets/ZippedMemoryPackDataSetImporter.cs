using System.IO.Compression;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.Misc;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets;

internal sealed class ZippedMemoryPackDataSetImporter(
	ImageLookupper imageLookupper,
	Factory<ImageSet> imageSetFactory,
	WriteRepository<ImageSet> imageSetsWriteRepository,
	WriteRepository<DataSet<Tag, Asset>> dataSetsWriteRepository,
	Deserializer<IReadOnlyCollection<ManagedImage>> imagesDeserializer,
	Deserializer<DataSet<Tag, Asset>> dataSetDeserializer)
	: DataSetImporter
{
	public async Task Import(string filePath)
	{
		await using var archiveStream = File.OpenRead(filePath);
		using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
		var imageSet = await ImportMissingImages(archive);
		if (imageSet.Images.Any())
			imageSetsWriteRepository.Add(imageSet);
		var dataSet = await ReadDataSet(archive);
		SetName(imageSet, $"Imported - {dataSet.Name}");
		dataSetsWriteRepository.Add(dataSet);
	}

	private async Task<ImageSet> ImportMissingImages(ZipArchive archive)
	{
		var images = await ImportImagesMetadata(archive);
		var imageSet = imageSetFactory.Create();
		var settableInitialImages = imageSet.GetInnermost<SettableInitialItems<ManagedImage>>();
		foreach (var image in images)
		{
			if (!IsMissing(image))
				continue;
			var wrappedImage = settableInitialImages.WrapAndInsert(image);
			await CopyImageData(archive, wrappedImage);
		}
		return imageSet;
	}

	private async Task<IReadOnlyCollection<ManagedImage>> ImportImagesMetadata(ZipArchive archive)
	{
		var entry = archive.GetEntry("images.bin");
		Guard.IsNotNull(entry);
		await using var stream = entry.Open();
		var images = await imagesDeserializer.DeserializeAsync(stream);
		Guard.IsNotNull(images);
		return images;
	}

	private bool IsMissing(ManagedImage image)
	{
		var imageId = GetId(image);
		return !imageLookupper.ContainsImage(imageId);
	}

	private static async Task CopyImageData(ZipArchive archive, ManagedImage image)
	{
		await using var readStream = OpenArchivedImageStream(archive, image);
		await using var writeStream = OpenImageWriteStream(image);
		await readStream.CopyToAsync(writeStream);
	}

	private static Stream OpenArchivedImageStream(ZipArchive archive, ManagedImage image)
	{
		var entry = GetArchivedImageEntry(archive, image);
		return entry.Open();
	}

	private static ZipArchiveEntry GetArchivedImageEntry(ZipArchive archive, ManagedImage image)
	{
		var entryPath = GetImageEntryPath(image);
		var entry = archive.GetEntry(entryPath);
		Guard.IsNotNull(entry);
		return entry;
	}

	private static string GetImageEntryPath(ManagedImage image)
	{
		var imageId = GetId(image);
		var fileExtensionProvider = image.GetFirst<FileExtensionProvider>();
		var fileName = $"{imageId.ToString()}.{fileExtensionProvider.FileExtension}";
		return $"images\\{fileName}";
	}

	private static Stream OpenImageWriteStream(ManagedImage image)
	{
		var streamableData = image.GetFirst<StreamableData>();
		return streamableData.OpenWriteStream();
	}

	private static Id GetId(ManagedImage image)
	{
		var idHolder = image.GetFirst<IdHolder>();
		return idHolder.Id;
	}

	private async Task<DataSet<Tag, Asset>> ReadDataSet(ZipArchive archive)
	{
		var entry = archive.GetEntry("data.bin");
		Guard.IsNotNull(entry);
		await using var stream = entry.Open();
		return await dataSetDeserializer.DeserializeAsync(stream);
	}

	private static void SetName(ImageSet imageSet, string name)
	{
		imageSet.GetInnermost<ImageSet>().Name = name;
	}
}