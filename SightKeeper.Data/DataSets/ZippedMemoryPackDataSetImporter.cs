using System.IO.Compression;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.DataSets;

public sealed class ZippedMemoryPackDataSetImporter(
	ImageLookupper imageLookupper,
	ReadRepository<StorableImageSet> imageSetsReadRepository,
	ImageSetFactory<StorableImageSet> imageSetFactory,
	WriteRepository<StorableImageSet> imageSetsWriteRepository,
	WriteRepository<DataSet> dataSetsWriteRepository,
	ReadRepository<DataSet> dataSetsReadRepository,
	ImageLookupperPopulator lookupperPopulator)
	: DataSetImporter
{
	public async Task Import(string filePath)
	{
		await using var archiveStream = File.OpenRead(filePath);
		using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
		await ImportMissingImages(archive);
		var dataSet = await ReadDataSet(archive);
		dataSet.Name = HandleDuplicate(dataSet.Name, dataSetsReadRepository.Items.Select(set => set.Name));
		dataSetsWriteRepository.Add(dataSet);
	}

	private async Task ImportMissingImages(ZipArchive archive)
	{
		var images = await ImportImagesMetadata(archive);
		StorableImageSet? importedImagesSet = null;
		foreach (var image in images)
		{
			if (imageLookupper.ContainsImage(image.Id))
				continue;
			importedImagesSet ??= GetOrCreateImportedImagesSet();
			var wrappedImage = importedImagesSet.WrapAndInsertImage(image);
			await CopyImageData(archive, wrappedImage);
			lookupperPopulator.AddImage(wrappedImage);
		}
	}

	private static async Task<IReadOnlyCollection<StorableImage>> ImportImagesMetadata(ZipArchive archive)
	{
		var entry = archive.GetEntry("images.bin");
		Guard.IsNotNull(entry);
		await using var stream = entry.Open();
		var images = await MemoryPackSerializer.DeserializeAsync<IReadOnlyCollection<StorableImage>>(stream);
		Guard.IsNotNull(images);
		return images;
	}

	private StorableImageSet GetOrCreateImportedImagesSet()
	{
		const string setName = "Imported";
		var set = imageSetsReadRepository.Items.FirstOrDefault(set => set.Name == setName);
		if (set != null)
			return set;
		set = imageSetFactory.CreateImageSet();
		set.Name = setName;
		imageSetsWriteRepository.Add(set);
		return set;
	}

	private static async Task CopyImageData(ZipArchive archive, StorableImage image)
	{
		var entryPath = Path.Combine("images", image.Id.ToString());
		if (!string.IsNullOrWhiteSpace(image.DataFormat))
			entryPath += $".{image.DataFormat}";
		var entry = archive.GetEntry(entryPath);
		if (entry == null)
			return;
		await using var readStream = entry.Open();
		await using var writeStream = image.OpenWriteStream();
		Guard.IsNotNull(writeStream);
		await readStream.CopyToAsync(writeStream);
	}

	private static async Task<DataSet> ReadDataSet(ZipArchive archive)
	{
		var entry = archive.GetEntry("data.bin");
		Guard.IsNotNull(entry);
		await using var stream = entry.Open();
		var dataSet = await MemoryPackSerializer.DeserializeAsync<DataSet>(stream);
		Guard.IsNotNull(dataSet);
		return dataSet;
	}

	public static string HandleDuplicate(string newName, IEnumerable<string> existingNames)
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