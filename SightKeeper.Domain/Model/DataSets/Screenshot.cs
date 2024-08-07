namespace SightKeeper.Domain.Model.DataSets;

public abstract class Screenshot
{
	public DateTime CreationDate { get; } = DateTime.Now;
	public abstract Asset? Asset { get; }
	public abstract ScreenshotsLibrary Library { get; }
	public virtual DataSet DataSet => Library.DataSet;

	protected internal abstract void DeleteFromLibrary();
}

public sealed class Screenshot<TAsset> : Screenshot where TAsset : Asset
{
	public override TAsset? Asset => _asset;
	public override AssetScreenshotsLibrary<TAsset> Library { get; }

	public Screenshot(AssetScreenshotsLibrary<TAsset> library)
	{
		Library = library;
	}

	internal void SetAsset(TAsset? asset)
	{
		_asset = asset;
	}

	protected internal override void DeleteFromLibrary()
	{
		Library.DeleteScreenshot(this);
	}

	private TAsset? _asset;
}