using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class AssetsLibrary
{
	public abstract IReadOnlyCollection<Asset> Assets { get; }
}

public sealed class AssetsLibrary<TAsset> : AssetsLibrary where TAsset : Asset
{
	public override IReadOnlyCollection<TAsset> Assets => _assets.Values;

	public TAsset MakeAsset(Screenshot screenshot)
	{
		var asset = _assetsFactory.CreateAsset();
		_assets.Add(screenshot, asset);
		return asset;
	}

	public void DeleteAsset(Screenshot screenshot)
	{
		var isRemoved = _assets.Remove(screenshot);
		Guard.IsTrue(isRemoved);
	}

	internal AssetsLibrary(AssetsFactory<TAsset> assetsFactory)
	{
		_assetsFactory = assetsFactory;
	}

	private readonly AssetsFactory<TAsset> _assetsFactory;
	private readonly Dictionary<Screenshot, TAsset> _assets = new();
}