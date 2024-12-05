using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class AssetsLibrary;

public sealed class AssetsLibrary<TAsset> : AssetsLibrary where TAsset : Asset
{
	public IReadOnlyDictionary<Screenshot, TAsset> Assets => _assets.AsReadOnly();

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