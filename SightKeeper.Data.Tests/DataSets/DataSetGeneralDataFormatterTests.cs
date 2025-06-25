using System.Buffers;
using FluentAssertions;
using MemoryPack;
using NSubstitute;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.Tests.Saving.DataSets;

public sealed class DataSetGeneralDataFormatterTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		var set = Substitute.For<DataSet>();
		set.Name.Returns(name);
		var persistedSet = Persist(set);
		persistedSet.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		const string description = "The description";
		var set = Substitute.For<DataSet>();
		set.Description.Returns(description);
		var persistedSet = Persist(set);
		persistedSet.Description.Should().Be(description);
	}

	private static DataSet Persist(DataSet set)
	{
		var buffer = Serialize(set);
		return Deserialize(buffer);
	}

	private static byte[] Serialize(DataSet set)
	{
		ArrayBufferWriter<byte> bufferWriter = new();
		using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var writer = new MemoryPackWriter<ArrayBufferWriter<byte>>(ref bufferWriter, state);
		DataSetGeneralDataFormatter.WriteGeneralData(ref writer, set);
		writer.Flush();
		return bufferWriter.WrittenSpan.ToArray();
	}

	private static DataSet Deserialize(ReadOnlySpan<byte> buffer)
	{
		using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var reader = new MemoryPackReader(buffer, state);
		InMemoryClassifierDataSet dataSet = new(new FakeTagFactory(), new StorableClassifierAssetFactory(), new StorableWeightsWrapper());
		DataSetGeneralDataFormatter.ReadGeneralData(ref reader, dataSet);
		return dataSet;
	}
}