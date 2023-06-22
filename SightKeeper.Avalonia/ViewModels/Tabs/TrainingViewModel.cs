using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Training;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class TrainingViewModel : ObservableObject
{
    private readonly ModelTrainer<Model> _trainer;
    public ReadOnlyObservableCollection<Model> AvailableModels { get; }
    [ObservableProperty] private Model? _selectedModel;
    [ObservableProperty] private bool _isTraining;

    [RelayCommand(CanExecute = nameof(CanStartTraining))]
    public async Task StartTraining()
    {
        if (SelectedModel == null) throw new NullReferenceException($"{nameof(SelectedModel)} is not set");
        _trainer.Train(SelectedModel, true);
    }

    public bool CanStartTraining() => !IsTraining;

    [RelayCommand]
    public async Task StopTraining()
    {
        _trainer.StopTraining();
    }

    public bool CanStopTraining() => IsTraining;

    public TrainingViewModel(Repository<Model> modelsRepository, ModelTrainer<Model> trainer)
    {
        _trainer = trainer;
        AvailableModels = modelsRepository.Items;
    }
}