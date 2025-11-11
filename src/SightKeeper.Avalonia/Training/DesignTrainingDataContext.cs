using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using SightKeeper.Application.Misc;
using SightKeeper.Application.Training;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Training;

internal sealed class DesignTrainingDataContext : TrainingDataContext
{
	public IReadOnlyCollection<DataSet<Tag, Asset>> DataSets => ReadOnlyCollection<DataSet<Tag, Asset>>.Empty;
	public DataSet<Tag, Asset>? DataSet { get; set; }
	public bool IsTraining => false;
	public ICommand StartTrainingCommand => CommandStubs.Enabled;
	public ICommand StopTrainingCommand => CommandStubs.Disabled;
	public IEnumerable<string> LogLines =>
	[
		"Data exported",
		"Training started"
	];

	public IObservable<object> Progress => Observable.Create<object>(async (observer, cancellationToken) =>
	{
		await Task.Delay(1000, cancellationToken);
		observer.OnNext("Exporting data");
		await Task.Delay(200, cancellationToken);
		await ProduceSequenceAsync("Exporting images", 500, () => Random.Shared.Next(33));
		observer.OnNext("Starting training");
		await Task.Delay(1000, cancellationToken);
		await ProduceSequenceAsync("Training", 50, () => 2000 + Random.Shared.Next(2000));
		observer.OnNext("Done");
		return;

		async Task ProduceSequenceAsync(string label, int count, Func<int> delayFactory)
		{
			var timeEstimator = new RemainingTimeEstimator(count);
			observer.OnNext(new Progress
			{
				Label = label,
				Total = count
			});
			for (int i = 0; i < count; i++)
			{
				await Task.Delay(delayFactory(), cancellationToken);
				var remainingTime = timeEstimator.Estimate(i + 1);
				observer.OnNext(new Progress
				{
					Label = label,
					Total = count,
					Current = i + 1,
					EstimatedTimeOfArrival = DateTime.Now + remainingTime
				});
			}
		}
	}).ObserveOn(new AvaloniaSynchronizationContext());
}