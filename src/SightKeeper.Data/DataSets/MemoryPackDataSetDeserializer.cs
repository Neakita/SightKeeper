using MemoryPack;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

public interface MemoryPackDataSetDeserializer
{
	DataSet<Tag, Asset> Deserialize(ref MemoryPackReader reader);
}