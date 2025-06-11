using FlakeId;
using SightKeeper.Application;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class FileSystemImageRepository : ImageRepository, IdRepository<Image>
{
	public FileSystemImageRepository(FileSystemDataAccess<Image> fileSystemDataAccess, ChangeListener changeListener, [Tag(typeof(AppData))] Lock appDataLock, WriteImageDataAccess writeImageDataAccess) : base(writeImageDataAccess)
	{
		_fileSystemDataAccess = fileSystemDataAccess;
		_changeListener = changeListener;
		_appDataLock = appDataLock;
	}

	public Id GetId(Image image)
	{
		return _fileSystemDataAccess.GetId(image);
	}

	public override void DeleteImagesRange(DomainImageSet set, int index, int count)
	{
		lock (_changeListener)
			base.DeleteImagesRange(set, index, count);
		_changeListener.SetDataChanged();
	}

	public void ClearUnassociatedImageFiles()
	{
		_fileSystemDataAccess.ClearUnassociatedFiles();
	}

	public void AssociateId(Image image, Id id)
	{
		_fileSystemDataAccess.AssociateId(image, id);
	}

	protected override Image CreateImage(
		DomainImageSet set,
		DateTimeOffset creationTimestamp,
		Vector2<ushort> resolution)
	{
		Image image;
		lock (_appDataLock)
			image = base.CreateImage(set, creationTimestamp, resolution);
		_changeListener.SetDataChanged();
		return image;
	}

	private readonly ChangeListener _changeListener;
	private readonly Lock _appDataLock;
	private readonly FileSystemDataAccess<Image> _fileSystemDataAccess;
}