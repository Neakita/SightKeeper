using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.Training;

internal sealed partial class TrainingViewModel : ViewModel, TrainingDataContext
{
	public IReadOnlyCollection<DataSet> DataSets { get; }
	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]public partial DataSet? DataSet { get; set; }
	[ObservableProperty] public partial ushort Width { get; set; } = 320;
	[ObservableProperty] public partial ushort Height { get; set; } = 320;

	ICommand TrainingDataContext.StartTrainingCommand => StartTrainingCommand;
	ICommand TrainingDataContext.StopTrainingCommand => StartTrainingCommand.CreateCancelCommand();

	public TrainingViewModel(ObservableListRepository<DataSet> dataSets, Trainer<AssetData> trainer)
	{
		_trainer = trainer;
		DataSets = dataSets.Items;
	}

	private readonly Trainer<AssetData> _trainer;
	private bool CanStartTraining => DataSet != null;

	[RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
	private async Task StartTraining(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(DataSet);
		_trainer.ImageSize = new Vector2<ushort>(Width, Height);
		var data = TrainData.Create(DataSet);
		await _trainer.TrainAsync(data, cancellationToken);
	}
}