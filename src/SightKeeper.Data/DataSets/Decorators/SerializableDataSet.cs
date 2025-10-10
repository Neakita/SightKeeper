using System.Buffers;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Decorators;

internal sealed class SerializableDataSet<TTag, TAsset>(
	DataSet<TTag, TAsset> inner,
	ushort unionTag,
	TagsFormatter<TTag> tagsFormatter,
	AssetsFormatter<TAsset> assetsFormatter)
	: DataSet<TTag, TAsset>, Decorator<DataSet<TTag, TAsset>>, MemoryPackSerializable
	where TTag : Tag
{
	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public string Description
	{
		get => inner.Description;
		set => inner.Description = value;
	}

	public TagsOwner<TTag> TagsLibrary => inner.TagsLibrary;
	public AssetsOwner<TAsset> AssetsLibrary => inner.AssetsLibrary;
	public WeightsLibrary WeightsLibrary => inner.WeightsLibrary;
	public DataSet<TTag, TAsset> Inner => inner;

	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteUnionHeader(unionTag);
		var tagIndexes = TagsLibrary.Tags.Index().ToDictionary(tuple => (Tag)tuple.Item, tuple => (byte)tuple.Index);
		DataSetGeneralDataFormatter.WriteGeneralData(ref writer, Name, Description);
		tagsFormatter.WriteTags(ref writer, TagsLibrary.Tags);
		assetsFormatter.Serialize(ref writer, AssetsLibrary.Assets, tagIndexes);
		WeightsFormatter.WriteWeights(ref writer, WeightsLibrary.Weights, tagIndexes);
	}
}