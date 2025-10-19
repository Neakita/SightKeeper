using FlakeId;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Vibrance.Changes;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class ObservableAssetsImage(ManagedImage inner) : ManagedImage, IDisposable
{
	public Id Id => inner.Id;
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;
	public Vector2<ushort> Size => inner.Size;
	public IReadOnlyCollection<Asset> Assets => _assets;
	public string? DataFormat => inner.DataFormat;

	public Image? Load(CancellationToken cancellationToken)
	{
		return null;
	}

	public Image<TPixel>? Load<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		return null;
	}

	public Task<Image?> LoadAsync(CancellationToken cancellationToken)
	{
		return inner.LoadAsync(cancellationToken);
	}

	public Task<Image<TPixel>?> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		return inner.LoadAsync<TPixel>(cancellationToken);
	}

	public Stream? OpenWriteStream()
	{
		return inner.OpenWriteStream();
	}

	public Stream? OpenReadStream()
	{
		return inner.OpenReadStream();
	}

	public bool TryCopyTo(string filePath)
	{
		return inner.TryCopyTo(filePath);
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
	}

	public override string? ToString()
	{
		return inner.ToString();
	}

	private readonly ExternalObservableCollection<Asset> _assets = new(inner.Assets);
}