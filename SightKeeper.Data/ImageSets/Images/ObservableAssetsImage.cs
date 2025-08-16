using FlakeId;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using Vibrance.Changes;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class ObservableAssetsImage(StorableImage inner) : StorableImage
{
	public Id Id => inner.Id;
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;
	public Vector2<ushort> Size => inner.Size;
	public IReadOnlyCollection<Asset> Assets => _assets;

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

	public void DeleteData()
	{
		inner.DeleteData();
	}

	public void Dispose()
	{
		_assets.Dispose();
		inner.Dispose();
	}

	public override string? ToString()
	{
		return inner.ToString();
	}

	private readonly ExternalObservableCollection<Asset> _assets = new(inner.Assets);
}