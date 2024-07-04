using System.Runtime.CompilerServices;
using FlakeId;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemScreenshotsDataAccess : ScreenshotsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public FileSystemScreenshotsDataAccess(IEnumerable<(ScreenshotsLibrary Library, IEnumerable<Id> ImageIds)> initialData)
	{
		foreach (var data in initialData)
		foreach (var imageId in data.ImageIds)
			_dataAccess.AssociateId(CreateScreenshotInLibrary(data.Library), imageId);

		[UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(CreateScreenshot))]
		static extern Screenshot CreateScreenshotInLibrary(ScreenshotsLibrary library);
	}

	public override Image LoadImage(Screenshot screenshot)
	{
		var data = _dataAccess.ReadAllBytes(screenshot);
		return CreateImage(data);

		[UnsafeAccessor(UnsafeAccessorKind.Constructor)]
		static extern Image CreateImage(byte[] data);
	}

	protected override void SaveScreenshotData(Screenshot screenshot, Image image)
	{
		_dataAccess.WriteAllBytes(screenshot, image.Data);
	}

	protected override void DeleteScreenshotData(Screenshot screenshot)
	{
		_dataAccess.Delete(screenshot);
	}

	private readonly FileSystemDataAccess<Screenshot> _dataAccess = new(".png");
}