using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Model.DataSets;

internal abstract class PackablePoserDataSet<TTag, TAsset> : PackableDataSet<TTag, TAsset, PackablePoserWeights>
	where TTag : PackableTag
	where TAsset : PackableAsset;