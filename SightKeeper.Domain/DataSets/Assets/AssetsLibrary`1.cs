using System.Diagnostics;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Domain.DataSets.Assets;

public sealed class AssetsLibrary<TAsset> : AssetsLibrary, AssetsOwner<TAsset> where TAsset : Asset
{
	public IReadOnlyDictionary<Screenshot, TAsset> Assets => _assets.AsReadOnly();

	public TAsset GetOrMakeAsset(Screenshot screenshot)
	{
		if (_assets.TryGetValue(screenshot, out var asset))
			return asset;
		return MakeAsset(screenshot);
	}

	public TAsset MakeAsset(Screenshot screenshot)
	{
		var asset = _assetsFactory.CreateAsset();
		screenshot.AddAsset(asset);
		_assets.Add(screenshot, asset);
		return asset;
	}

	public void DeleteAsset(Screenshot screenshot)
	{
		var isRemoved = _assets.Remove(screenshot, out var asset);
		if (!isRemoved)
			throw new ArgumentException("The asset associated with the specified screenshot was not found", nameof(screenshot));
		Debug.Assert(asset != null);
		screenshot.RemoveAsset(asset);
	}

	public void ClearAssets()
	{
		foreach (var (screenshot, asset) in _assets)
			screenshot.RemoveAsset(asset);
		_assets.Clear();
	}

	public override bool Contains(Screenshot screenshot)
	{
		return _assets.ContainsKey(screenshot);
	}

	public TAsset? GetOptionalAsset(Screenshot screenshot)
	{
		_assets.TryGetValue(screenshot, out var asset);
		return asset;
	}

	internal AssetsLibrary(AssetsFactory<TAsset> assetsFactory)
	{
		_assetsFactory = assetsFactory;
	}

	private readonly AssetsFactory<TAsset> _assetsFactory;
	private readonly Dictionary<Screenshot, TAsset> _assets = new();
}