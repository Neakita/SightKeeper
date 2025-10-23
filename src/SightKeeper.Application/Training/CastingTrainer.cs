﻿using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training;

internal sealed class CastingTrainer<TTag, TAsset>(Trainer<TTag, TAsset> inner) : Trainer<ReadOnlyTag, ReadOnlyAsset>
{
	public Vector2<ushort> ImageSize
	{
		get => inner.ImageSize;
		set => inner.ImageSize = value;
	}

	public Task TrainAsync(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyAsset> data, CancellationToken cancellationToken)
	{
		return inner.TrainAsync((ReadOnlyDataSet<TTag, TAsset>)data, cancellationToken);
	}
}