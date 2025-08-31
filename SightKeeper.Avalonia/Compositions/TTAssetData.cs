using System;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Compositions;

internal sealed class TTAssetData : ReadOnlyAsset
{
	public ImageData Image => throw new NotSupportedException();
	public AssetUsage Usage => throw new NotSupportedException();
}