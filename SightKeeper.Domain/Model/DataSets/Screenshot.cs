using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class Screenshot
{
	public DateTime CreationDate { get; } = DateTime.Now;
	public Asset? Asset { get; internal set; }
	public ScreenshotsLibrary Library { get; }

	public Screenshot(ScreenshotsLibrary library)
	{
		Library = library;
	}

	public Asset MakeAsset()
	{
		Guard.IsNull(Asset);
		Asset = new Asset(this);
		Library.DataSet.Assets.AddAsset(Asset);
		return Asset;
	}

	public void DeleteAsset()
	{
		Guard.IsNotNull(Asset);
		Library.DataSet.Assets.DeleteAsset(Asset);
	}
}