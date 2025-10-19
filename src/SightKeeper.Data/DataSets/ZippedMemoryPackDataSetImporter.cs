using System.IO.Compression;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
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
	ReadRepository<DataSet<Tag, Asset>> dataSetsReadRepository,
	Deserializer<IReadOnlyCollection<ManagedImage>> imagesDeserializer,
	Deserializer<DataSet<Tag, Asset>> dataSetDeserializer)
	: DataSetImporter
{
	public async Task Import(string filePath)
	{
		await using var archiveStream = File.OpenRead(filePath);
		using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
		var importedImageSet = await ImportMissingImages(archive);
		var dataSet = await ReadDataSet(archive);
		dataSet.Name = HandleDuplicate(dataSet.Name, dataSetsReadRepository.Items.Select(set => set.Name));
		if (importedImageSet != null)
			importedImageSet.Name = $"Imported - {dataSet.Name}";
		dataSetsWriteRepository.Add(dataSet);
	}

	private async Task<ImageSet?> ImportMissingImages(ZipArchive archive)
	{
		var images = await ImportImagesMetadata(archive);
		ImageSet? importedImagesSet = null;
		foreach (var image in images)
		{
			var idHolder = image.GetFirst<IdHolder>();
			var imageId = idHolder.Id;
			if (imageLookupper.ContainsImage(imageId))
				continue;
			importedImagesSet ??= CreateImagesSet();
			var settableInitialImages = importedImagesSet.GetInnermost<SettableInitialItems<ManagedImage>>();
			var wrappedImage = settableInitialImages.WrapAndInsert(image);
			await CopyImageData(archive, wrappedImage);
		}
		return importedImagesSet;
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

	private ImageSet CreateImagesSet()
	{
		var set = imageSetFactory.Create();
		imageSetsWriteRepository.Add(set);
		return set;
	}

	private static async Task CopyImageData(ZipArchive archive, ManagedImage image)
	{
		var idHolder = image.GetFirst<IdHolder>();
		var imageId = idHolder.Id;
		var fileExtensionProvider = image.GetFirst<FileExtensionProvider>();
		var fileName = $"{imageId.ToString()}.{fileExtensionProvider}";
		var entryPath = Path.Combine("images", fileName);
		var entry = archive.GetEntry(entryPath);
		if (entry == null)
			return;
		await using var readStream = entry.Open();
		var streamableData = image.GetFirst<StreamableData>();
		await using var writeStream = streamableData.OpenWriteStream();
		Guard.IsNotNull(writeStream);
		await readStream.CopyToAsync(writeStream);
	}

	private async Task<DataSet<Tag, Asset>> ReadDataSet(ZipArchive archive)
	{
		var entry = archive.GetEntry("data.bin");
		Guard.IsNotNull(entry);
		await using var stream = entry.Open();
		var dataSet = await dataSetDeserializer.DeserializeAsync(stream);
		Guard.IsNotNull(dataSet);
		return dataSet;
	}

	private static string HandleDuplicate(string newName, IEnumerable<string> existingNames)
	{
		var regex = new Regex(@"^(.*?)\s*((\d+))?$");
		newName = regex.Match(newName).Groups[1].Captures.Single().Value;
		var usedIndexes = existingNames.Select(name => regex.Match(name))
			.Where(match => match.Groups[1].Captures.Single().Value == newName)
			.Select(match => match.Groups[3].Captures.SingleOrDefault()?.Value)
			.Select(index => string.IsNullOrWhiteSpace(index) ? 0 : int.Parse(index))
			.ToHashSet();
		if (!usedIndexes.Contains(0))
			return newName;
		for (int i = 1;; i++)
			if (!usedIndexes.Contains(i))
				return $"{newName} {i}";
	}
}