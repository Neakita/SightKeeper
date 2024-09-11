using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DataSet{TTag,TKeyPointTag,TAsset}"/>
/// </summary>
internal abstract class PackablePoserDataSet<TTag, TAsset> : PackableDataSet<TTag, TAsset>
	where TTag : PackableTag
	where TAsset : PackableAsset;