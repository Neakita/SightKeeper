using Autofac;
using SightKeeper.Application.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training;

internal sealed class LifetimeTrainer(LifetimeScopeProvider lifetimeScopeProvider, ILifetimeScope lifetimeScope) : Trainer<ReadOnlyTag, ReadOnlyAsset>
{
	public async Task TrainAsync(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyAsset> data, CancellationToken cancellationToken)
	{
		await using var dataScope = lifetimeScopeProvider.BeginLifetimeScope(data, lifetimeScope);
		await using var trainingScope = dataScope.BeginLifetimeScope(typeof(LifetimeTrainer));
		var trainer = trainingScope.Resolve<Trainer<ReadOnlyTag, ReadOnlyAsset>>();
		await trainer.TrainAsync(data, cancellationToken);
	}
}