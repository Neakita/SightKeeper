using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Poser2DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DDataSet : PackablePoserDataSet<PackablePoser2DTag, PackableItemsAsset<PackablePoser2DItem>>;