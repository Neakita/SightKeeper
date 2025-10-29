using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using Vibrance.Changes;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class ObservableAssetsImage(ManagedImage inner) : ManagedImage, EditableImageAssets, Decorator<ManagedImage>, IDisposable
{
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;
	public Vector2<ushort> Size => inner.Size;
	public IReadOnlyCollection<Asset> Assets => _assets;
	public ManagedImage Inner => inner;

	public void Add(Asset asset)
	{
		inner.GetFirst<EditableImageAssets>().Add(asset);
		if (!_assets.HasObservers)
			return;
		Addition<Asset> change = new()
		{
			Items = [asset]
		};
		_assets.Notify(change);
	}

	public void Remove(Asset asset)
	{
		inner.GetFirst<EditableImageAssets>().Remove(asset);
		if (!_assets.HasObservers)
			return;
		Removal<Asset> change = new()
		{
			Items = [asset]
		};
		_assets.Notify(change);
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