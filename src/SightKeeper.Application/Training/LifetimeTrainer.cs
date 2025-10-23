using Autofac;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training;

internal sealed class LifetimeTrainer(LifetimeScopeProvider lifetimeScopeProvider) : Trainer<ReadOnlyTag, ReadOnlyAsset>
{
	public Vector2<ushort> ImageSize { get; set; }

	public async Task TrainAsync(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyAsset> data, CancellationToken cancellationToken)
	{
		var dataScope = lifetimeScopeProvider.GetLifetimeScope(data);
		await using var trainingScope = dataScope.BeginLifetimeScope();
		var trainer = trainingScope.Resolve<Trainer<ReadOnlyTag, ReadOnlyAsset>>();
		trainer.ImageSize = ImageSize;
		await trainer.TrainAsync(data, cancellationToken);
	}
}