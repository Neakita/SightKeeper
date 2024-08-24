using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public abstract class Screenshot
{
	public DateTime CreationDate { get; }
	public abstract Asset? Asset { get; }
	public abstract ScreenshotsLibrary Library { get; }
	public virtual DataSet DataSet => Library.DataSet;

	protected Screenshot(DateTime creationDate)
	{
		CreationDate = creationDate;
	}

	public abstract void DeleteFromLibrary();
}

public sealed class Screenshot<TAsset> : Screenshot where TAsset : Asset
{
	public override TAsset? Asset => _asset;
	public override ScreenshotsLibrary<TAsset> Library { get; }

	public Screenshot(ScreenshotsLibrary<TAsset> library, DateTime creationDate) : base(creationDate)
	{
		Library = library;
	}

	internal void SetAsset(TAsset? asset)
	{
		_asset = asset;
	}

	public override void DeleteFromLibrary()
	{
		Library.DeleteScreenshot(this);
	}

	private TAsset? _asset;
}