using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.Training;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Training;

internal sealed partial class TrainingViewModel : ViewModel, TrainingDataContext
{
	public IReadOnlyCollection<DataSet<Tag, Asset>> DataSets { get; }
	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]public partial DataSet<Tag, Asset>? DataSet { get; set; }
	[ObservableProperty] public partial ushort Width { get; set; } = 320;
	[ObservableProperty] public partial ushort Height { get; set; } = 320;

	ICommand TrainingDataContext.StartTrainingCommand => StartTrainingCommand;
	ICommand TrainingDataContext.StopTrainingCommand => StartTrainingCommand.CreateCancelCommand();

	public TrainingViewModel(ObservableListRepository<DataSet<Tag, Asset>> dataSets, Trainer<ReadOnlyTag, ReadOnlyAsset> trainer)
	{
		_trainer = trainer;
		DataSets = dataSets.Items;
	}

	private readonly Trainer<ReadOnlyTag, ReadOnlyAsset> _trainer;
	private bool CanStartTraining => DataSet != null;

	[RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
	private async Task StartTraining(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(DataSet);
		_trainer.ImageSize = new Vector2<ushort>(Width, Height);
		await _trainer.TrainAsync(DataSet, cancellationToken);
	}
}