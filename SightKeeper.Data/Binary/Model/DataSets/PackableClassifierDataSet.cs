using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="ClassifierDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableClassifierDataSet : PackableDataSet<PackableTag, PackableClassifierAsset, PackablePlainWeights>;