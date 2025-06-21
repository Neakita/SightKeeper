using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class ClassifierDataSetFormatter : MemoryPackFormatter<ClassifierDataSet>
{
	public ClassifierDataSetFormatter()
	{
		_tagFactory = new StorableTagFactory();
		_assetFactory = new StorableClassifierAssetFactory();
		_setWrapper = new ClassifierDataSetWrapper();
	}

	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer,
		scoped ref ClassifierDataSet? dataSet)
	{
		if (dataSet == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}

		var tagIndexes = dataSet.TagsLibrary.Tags.Index().ToDictionary(tuple => tuple.Item, tuple => (byte)tuple.Index);

		writer.WriteObjectHeader(5);
		writer.WriteString(dataSet.Name);
		writer.WriteString(dataSet.Description);

		writer.WriteCollectionHeader(dataSet.TagsLibrary.Tags.Count);
		foreach (var tag in dataSet.TagsLibrary.Tags)
		{
			writer.WriteObjectHeader(2);
			writer.WriteString(tag.Name);
			writer.WriteUnmanaged(tag.Color);
		}

		writer.WriteCollectionHeader(dataSet.AssetsLibrary.Assets.Count);
		foreach (var asset in dataSet.AssetsLibrary.Assets)
		{
			var imageId = asset.Image.GetId();
			var tagIndex = tagIndexes[asset.Tag];
			writer.WriteUnmanagedWithObjectHeader(3, imageId, tagIndex, asset.Usage);
		}

		writer.WriteCollectionHeader(dataSet.WeightsLibrary.Weights.Count);
		foreach (var weights in dataSet.WeightsLibrary.Weights)
		{
			writer.WriteUnmanagedWithObjectHeader(
				7,
				/*weights.Id*/ default(Id),
				weights.Model,
				weights.CreationTimestamp,
				weights.ModelSize,
				weights.Metrics,
				weights.Resolution);
			var weightsTagIndexes = ArrayPool<byte>.Shared.Rent(weights.Tags.Count);
			for (int i = 0; i < weights.Tags.Count; i++)
			{
				var tag = weights.Tags[i];
				weightsTagIndexes[i] = tagIndexes[tag];
			}

			writer.WriteUnmanagedSpan(weightsTagIndexes.AsSpan(0, weights.Tags.Count));
		}
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref ClassifierDataSet? set)
	{
		if (!reader.TryReadObjectHeader(out var setMemberCount))
		{
			set = null;
			return;
		}

		Guard.IsEqualTo<byte>(setMemberCount, 5);
		var setName = reader.ReadString();
		Guard.IsNotNull(setName);
		var setDescription = reader.ReadString();
		Guard.IsNotNull(setDescription);

		var inMemorySet = new InMemoryClassifierDataSet(_tagFactory, _assetFactory)
		{
			Name = setName,
			Description = setDescription
		};

		Guard.IsTrue(reader.TryReadCollectionHeader(out var tagsCount));
		inMemorySet.TagsLibrary.EnsureCapacity(tagsCount);
		for (int i = 0; i < tagsCount; i++)
		{
			Guard.IsTrue(reader.TryReadObjectHeader(out var tagMemberCount));
			Guard.IsEqualTo<byte>(tagMemberCount, 2);
			var tagName = reader.ReadString();
			Guard.IsNotNull(tagName);
			var color = reader.ReadUnmanaged<uint>();
			var tag = inMemorySet.TagsLibrary.CreateTag(tagName);
			tag.Color = color;
		}

		Guard.IsTrue(reader.TryReadCollectionHeader(out var assetsCount));
		inMemorySet.AssetsLibrary.EnsureCapacity(assetsCount);
		for (int i = 0; i < assetsCount; i++)
		{
			Guard.IsTrue(reader.TryReadObjectHeader(out var assetMemberCount));
			Guard.IsEqualTo<byte>(assetMemberCount, 3);
			reader.ReadUnmanaged(out Id imageId, out byte tagIndex, out AssetUsage usage);
			var image = GetImage(imageId);
			var asset = inMemorySet.AssetsLibrary.MakeAsset(image);
			asset.Tag = inMemorySet.TagsLibrary.Tags[tagIndex];
			asset.Usage = usage;
		}

		Guard.IsTrue(reader.TryReadCollectionHeader(out var weightsCount));
		inMemorySet.WeightsLibrary.EnsureCapacity(weightsCount);
		for (int i = 0; i < weightsCount; i++)
		{
			Guard.IsTrue(reader.TryReadObjectHeader(out var weightsPropertyCount));
			Guard.IsEqualTo<byte>(weightsPropertyCount, 7);
			reader.ReadUnmanaged(
				out Id id,
				out Model model,
				out DateTimeOffset creationTimestamp,
				out ModelSize modelSize,
				out WeightsMetrics metrics,
				out Vector2<ushort> resolution);
			Span<byte> tagIndexes = new();
			reader.ReadUnmanagedSpan(ref tagIndexes);
			List<Tag> tags = new(tagIndexes.Length);
			foreach (var tagIndex in tagIndexes)
			{
				var tag = inMemorySet.TagsLibrary.Tags[tagIndex];
				tags.Add(tag);
			}

			var weights = CreateWeights(id, model, creationTimestamp, modelSize, metrics, resolution, tags);
			inMemorySet.WeightsLibrary.AddWeights(weights);
		}

		set = _setWrapper.Wrap(inMemorySet);
	}

	private readonly TagFactory<Tag> _tagFactory;
	private readonly AssetFactory<ClassifierAsset> _assetFactory;
	private readonly ClassifierDataSetWrapper _setWrapper;

	private Image GetImage(Id id)
	{
		throw new NotImplementedException();
	}

	private Domain.DataSets.Weights.Weights CreateWeights(
		Id id,
		Model model,
		DateTimeOffset creationTimestamp,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<Tag> tags)
	{
		throw new NotImplementedException();
	}
}