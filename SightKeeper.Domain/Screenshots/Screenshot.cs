using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.Screenshots;

public sealed class Screenshot
{
	public DateTimeOffset CreationDate { get; }
	public Vector2<ushort> ImageSize { get; }
	public IReadOnlyCollection<Asset> Assets => new ReadOnlySet<Asset>(_assets);

	internal Screenshot(DateTimeOffset creationDate, Vector2<ushort> imageSize)
	{
		CreationDate = creationDate;
		Guard.IsGreaterThan<ushort>(imageSize.X, 0);
		Guard.IsGreaterThan<ushort>(imageSize.Y, 0);
		ImageSize = imageSize;
	}

	internal void AddAsset(Asset asset)
	{
		bool isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveAsset(Asset asset)
	{
		bool isRemoved = _assets.Remove(asset);
		Guard.IsTrue(isRemoved);
	}

	private readonly HashSet<Asset> _assets = new();
}