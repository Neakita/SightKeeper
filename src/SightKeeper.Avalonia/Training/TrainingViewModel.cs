using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using SightKeeper.Application.Misc;
using SightKeeper.Application.Training;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Training;

internal sealed partial class TrainingViewModel : ViewModel, TrainingDataContext, IDisposable, IAsyncDisposable
{
	public IReadOnlyCollection<DataSet<Tag, Asset>> DataSets { get; }
	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
	public partial DataSet<Tag, Asset>? DataSet { get; set; }
	[ObservableProperty] public partial bool IsTraining { get; private set; }

	ICommand TrainingDataContext.StartTrainingCommand => StartTrainingCommand;
	ICommand TrainingDataContext.StopTrainingCommand => StartTrainingCommand.CreateCancelCommand();
	public IEnumerable<string> LogLines { get; }

	public TrainingViewModel(ObservableListRepository<DataSet<Tag, Asset>> dataSets, ILifetimeScope lifetime)
	{
		var logLines = new AvaloniaList<string>();
		var sink = new CollectionSink(logLines)
		{
			MaximumEntries = 100,
			RemoveRange = logLines.RemoveRange
		};
		var logger = new LoggerConfiguration()
			.WriteTo.Sink(sink)
			.CreateLogger();
		LogLines = logLines;
		_lifetime = lifetime.BeginLifetimeScope(typeof(TrainingViewModel), builder =>
		{
			builder.RegisterInstance(logger)
				.As<ILogger>();
		});
		_trainer = _lifetime.Resolve<Trainer<ReadOnlyTag, ReadOnlyAsset>>();
		_logger = _lifetime.Resolve<ILogger>();
		DataSets = dataSets.Items;
	}

	public void Dispose()
	{
		_lifetime.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		await _lifetime.DisposeAsync();
	}

	private readonly Trainer<ReadOnlyTag, ReadOnlyAsset> _trainer;
	private readonly ILogger _logger;
	private readonly ILifetimeScope _lifetime;
	private bool CanStartTraining => DataSet != null;

	[RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
	private async Task StartTraining(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(DataSet);
		IsTraining = true;
		try
		{
			await _trainer.TrainAsync(DataSet, cancellationToken);
		}
		catch (OperationCanceledException exception)
		{
			_logger.Debug(exception, "Training was cancelled");
		}
		finally
		{
			IsTraining = false;
		}
	}
}