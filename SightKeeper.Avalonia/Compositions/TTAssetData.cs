using System;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Compositions;

internal sealed class TTAssetData : AssetData
{
	public ImageData Image => throw new NotSupportedException();
	public AssetUsage Usage => throw new NotSupportedException();
}