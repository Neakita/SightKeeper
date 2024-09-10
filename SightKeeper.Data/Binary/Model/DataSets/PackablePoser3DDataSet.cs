using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Poser3DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DDataSet : PackablePoserDataSet<PackablePoser3DTag, PackableItemsAsset<PackablePoser3DItem>>;