using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using Vibrance.Changes;

namespace SightKeeper.Data.Images;

internal sealed class ObservableAssetsImage(Image inner) : Image, Decorator<Image>
{
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;

	public Vector2<ushort> Size => inner.Size;

	public IReadOnlyCollection<Asset> Assets => _assets;

	public Image Inner => inner;

	public Stream? OpenWriteStream()
	{
		return inner.OpenWriteStream();
	}

	public Stream? OpenReadStream()
	{
		return inner.OpenReadStream();
	}

	public void AddAsset(Asset asset)
	{
		inner.AddAsset(asset);
		if (!_assets.HasObservers)
			return;
		Addition<Asset> change = new()
		{
			Items = [asset]
		};
		_assets.Notify(change);
	}

	public void RemoveAsset(Asset asset)
	{
		inner.RemoveAsset(asset);
		if (!_assets.HasObservers)
			return;
		Removal<Asset> change = new()
		{
			Items = [asset]
		};
		_assets.Notify(change);
	}

	private readonly ExternalObservableCollection<Asset> _assets = new(inner.Assets);
}