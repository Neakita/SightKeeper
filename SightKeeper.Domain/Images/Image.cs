using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.Images;

public sealed class Image
{
	public DateTimeOffset CreationTimestamp { get; }
	public Vector2<ushort> ImageSize { get; }
	public IReadOnlyCollection<Asset> Assets => _assets.AsReadOnly();

	internal Image(DateTimeOffset creationTimestamp, Vector2<ushort> imageSize)
	{
		ValidateImageSize(imageSize);
		CreationTimestamp = creationTimestamp;
		ImageSize = imageSize;
	}

	internal void AddAsset(Asset asset)
	{
		bool isAdded = _assets.Add(asset);
		if (!isAdded)
			throw new ArgumentException("An asset already exists");
	}

	internal void RemoveAsset(Asset asset)
	{
		bool isRemoved = _assets.Remove(asset);
		if (!isRemoved)
			throw new ArgumentException("Specified asset was not found and therefore not deleted");
	}

	private readonly HashSet<Asset> _assets = new();

	private void ValidateImageSize(Vector2<ushort> imageSize)
	{
		ValidateImageSize(imageSize.X);
		ValidateImageSize(imageSize.Y);
	}

	private void ValidateImageSize(ushort imageSize)
	{
		if (imageSize <= 0)
			throw new ArgumentException($"Image size should be greater than 0, but was {imageSize}");
	}
}