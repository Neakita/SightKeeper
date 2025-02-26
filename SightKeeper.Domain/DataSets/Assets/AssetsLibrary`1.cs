using System.Diagnostics;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public sealed class AssetsLibrary<TAsset> : AssetsLibrary, AssetsOwner<TAsset> where TAsset : Asset
{
	public IReadOnlyDictionary<Image, TAsset> Assets => _assets.AsReadOnly();

	public TAsset GetOrMakeAsset(Image image)
	{
		if (_assets.TryGetValue(image, out var asset))
			return asset;
		return MakeAsset(image);
	}

	public TAsset MakeAsset(Image image)
	{
		var asset = _assetsFactory.CreateAsset();
		image.AddAsset(asset);
		_assets.Add(image, asset);
		return asset;
	}

	public void DeleteAsset(Image image)
	{
		var isRemoved = _assets.Remove(image, out var asset);
		if (!isRemoved)
			throw new ArgumentException("The asset associated with the specified image was not found", nameof(image));
		Debug.Assert(asset != null);
		image.RemoveAsset(asset);
	}

	public void ClearAssets()
	{
		foreach (var (image, asset) in _assets)
			image.RemoveAsset(asset);
		_assets.Clear();
	}

	public override bool Contains(Image image)
	{
		return _assets.ContainsKey(image);
	}

	public TAsset? GetOptionalAsset(Image image)
	{
		_assets.TryGetValue(image, out var asset);
		return asset;
	}

	internal AssetsLibrary(AssetsFactory<TAsset> assetsFactory)
	{
		_assetsFactory = assetsFactory;
	}

	private readonly AssetsFactory<TAsset> _assetsFactory;
	private readonly Dictionary<Image, TAsset> _assets = new();
}