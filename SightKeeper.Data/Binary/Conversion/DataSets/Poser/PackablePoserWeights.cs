using System.Collections.ObjectModel;
using SightKeeper.Data.Binary.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser;

internal sealed class PackablePoserWeights : PackableWeights
{
	public required ReadOnlyDictionary<byte, ReadOnlyCollection<byte>> TagsIndexes { get; init; }
}