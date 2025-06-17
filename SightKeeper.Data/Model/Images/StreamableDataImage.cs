using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class StreamableDataImage(PackableImage inner, FileSystemDataAccess dataAccess) : Image
{
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;

	public Vector2<ushort> Size => inner.Size;

	public IReadOnlyCollection<Asset> Assets => inner.Assets;

	public Stream? OpenReadDataStream()
	{
		return dataAccess.OpenRead(inner.Id);
	}

	public Stream? OpenWriteDataStream()
	{
		return dataAccess.OpenWrite(inner.Id);
	}
}