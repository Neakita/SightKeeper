using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class ClassifierAssetsFormatter(ImageLookupper imageLookupper, TagIndexProvider tagIndexProvider) : AssetsFormatter<ClassifierAsset>
{
	public void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<ClassifierAsset> assets)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(assets.Count);
		foreach (var asset in assets)
		{
			var image = asset.Image;
			var idHolder = image.GetFirst<IdHolder>();
			var imageId = idHolder.Id;
			var tagIndex = tagIndexProvider.GetTagIndex(asset.Tag);
			writer.WriteUnmanaged(imageId, tagIndex, asset.Usage);
		}
	}

	public void Deserialize(ref MemoryPackReader reader, DataSet<Tag, ClassifierAsset> set)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var assetsCount));
		for (int i = 0; i < assetsCount; i++)
		{
			reader.ReadUnmanaged(out Id imageId, out byte tagIndex, out AssetUsage usage);
			var image = imageLookupper.GetImage(imageId);
			var asset = set.AssetsLibrary.MakeAsset(image);
			var tag = set.TagsLibrary.Tags[tagIndex];
			var innermostAsset = asset.GetInnermost<ClassifierAsset>();
			innermostAsset.Tag = tag;
			tag.GetFirst<EditableTagUsers>().AddUser(asset);
			innermostAsset.Usage = usage;
		}
	}
}