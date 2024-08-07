using System.Runtime.CompilerServices;
using FlakeId;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemScreenshotsDataAccess : ScreenshotsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public override Image LoadImage(Screenshot screenshot)
	{
		var data = _dataAccess.ReadAllBytes(screenshot);
		return CreateImage(data);

		[UnsafeAccessor(UnsafeAccessorKind.Constructor)]
		static extern Image CreateImage(byte[] data);
	}

	public Id GetId(Screenshot screenshot)
	{
		return _dataAccess.GetId(screenshot);
	}

	internal void AssociateId(Screenshot screenshot, Id id)
	{
		_dataAccess.AssociateId(screenshot, id);
	}

	internal Screenshot<TAsset> CreateExistingScreenshot<TAsset>(AssetScreenshotsLibrary<TAsset> library)
		where TAsset : Asset
	{
		_ignoreNewScreenshots = true;
		try
		{
			return CreateScreenshot(library, []);
		}
		finally
		{
			_ignoreNewScreenshots = false;
		}
	}

	protected override void SaveScreenshotData(Screenshot screenshot, Image image)
	{
		if (_ignoreNewScreenshots)
			return;
		_dataAccess.WriteAllBytes(screenshot, image.Data);
	}

	protected override void DeleteScreenshotData(Screenshot screenshot)
	{
		_dataAccess.Delete(screenshot);
	}

	private readonly FileSystemDataAccess<Screenshot> _dataAccess = new(".png");
	private bool _ignoreNewScreenshots;
}