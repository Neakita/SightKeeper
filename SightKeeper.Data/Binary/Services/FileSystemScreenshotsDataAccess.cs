using FlakeId;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemScreenshotsDataAccess : ScreenshotsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public override FileStream LoadImage(Screenshot screenshot)
	{
		return _dataAccess.OpenReadStream(screenshot);
	}

	public Id GetId(Screenshot screenshot)
	{
		return _dataAccess.GetId(screenshot);
	}

	internal void AssociateId(Screenshot screenshot, Id id)
	{
		_dataAccess.AssociateId(screenshot, id);
	}

	protected override void SaveScreenshotData(Screenshot screenshot, byte[] data)
	{
		_dataAccess.WriteAllBytes(screenshot, data);
	}

	protected override void DeleteScreenshotData(Screenshot screenshot)
	{
		_dataAccess.Delete(screenshot);
	}

	private readonly FileSystemDataAccess<Screenshot> _dataAccess = new(".png");
}