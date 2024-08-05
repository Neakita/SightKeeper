namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DScreenshot : Screenshot
{
	public override Poser3DAsset? Asset => _asset;
	public override Poser3DScreenshotsLibrary Library { get; }
	public override Poser3DDataSet DataSet => Library.DataSet;

	internal Poser3DScreenshot(Poser3DScreenshotsLibrary library)
	{
		Library = library;
	}

	internal void SetAsset(Poser3DAsset? asset)
	{
		_asset = asset;
	}

	protected internal override void DeleteFromLibrary()
	{
		Library.DeleteScreenshot(this);
	}

	private Poser3DAsset? _asset;
}