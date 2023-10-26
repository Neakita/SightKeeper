using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class WeightsAsset : ObservableObject
{
	public Asset Asset { get; private set; }
	public Weights Weights { get; private set; }

	public WeightsAsset(Asset asset, Weights weights)
	{
		Asset = asset;
		Weights = weights;
	}

	private WeightsAsset()
	{
		Asset = null!;
		Weights = null!;
	}
}