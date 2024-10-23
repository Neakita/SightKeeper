using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public abstract class Screenshot
{
	public DateTimeOffset CreationDate { get; }
	public abstract Asset? Asset { get; }
	public abstract ScreenshotsLibrary Library { get; }
	public DataSet DataSet => Library.DataSet;
	public Vector2<ushort> ImageSize { get; }

	protected Screenshot(DateTimeOffset creationDate, Vector2<ushort> imageSize)
	{
		CreationDate = creationDate;
		Guard.IsGreaterThan<ushort>(imageSize.X, 0);
		Guard.IsGreaterThan<ushort>(imageSize.Y, 0);
		ImageSize = imageSize;
	}

	public abstract void DeleteFromLibrary();
}

public sealed class Screenshot<TAsset> : Screenshot where TAsset : Asset
{
	public override TAsset? Asset => _asset;
	public override ScreenshotsLibrary<TAsset> Library { get; }

	public Screenshot(ScreenshotsLibrary<TAsset> library, DateTimeOffset creationDate, Vector2<ushort> imageSize)
		: base(creationDate, imageSize)
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