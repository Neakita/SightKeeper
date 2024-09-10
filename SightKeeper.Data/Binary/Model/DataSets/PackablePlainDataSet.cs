using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Model.DataSets;

internal abstract class PackablePlainDataSet<TAsset> : PackableDataSet<PackableTag, TAsset, PackablePlainWeights>
	where TAsset : PackableAsset;