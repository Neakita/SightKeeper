namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DScreenshot : Screenshot
{
	public override Poser2DAsset? Asset => _asset;
	public override Poser2DScreenshotsLibrary Library { get; }
	public override Poser2DDataSet DataSet => Library.DataSet;

	internal Poser2DScreenshot(Poser2DScreenshotsLibrary library)
	{
		Library = library;
	}

	internal void SetAsset(Poser2DAsset? asset)
	{
		_asset = asset;
	}

	protected internal override void DeleteFromLibrary()
	{
		Library.DeleteScreenshot(this);
	}

	private Poser2DAsset? _asset;
}