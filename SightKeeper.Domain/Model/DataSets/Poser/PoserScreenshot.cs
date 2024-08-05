namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserScreenshot : Screenshot
{
	public override PoserAsset? Asset => _asset;
	public override PoserScreenshotsLibrary Library { get; }
	public override PoserDataSet DataSet => Library.DataSet;

	internal PoserScreenshot(PoserScreenshotsLibrary library)
	{
		Library = library;
	}

	internal void SetAsset(PoserAsset? asset)
	{
		_asset = asset;
	}

	protected internal override void DeleteFromLibrary()
	{
		Library.DeleteScreenshot(this);
	}

	private PoserAsset? _asset;
}