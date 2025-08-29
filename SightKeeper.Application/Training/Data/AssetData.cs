using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Data;

public interface AssetData
{
	ImageData Image { get; }
	AssetUsage Usage { get; }
}