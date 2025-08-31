using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Training.Data;

public interface AssetData
{
	ImageData Image { get; }
	AssetUsage Usage { get; }
}