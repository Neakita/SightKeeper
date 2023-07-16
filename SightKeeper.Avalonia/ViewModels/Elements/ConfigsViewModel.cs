using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed partial class ConfigsViewModel : ViewModel, IConfigsViewModel
{
    public Task<IReadOnlyCollection<ModelConfig>> Configs => _dataAccess.GetConfigs();
    
    public ConfigsViewModel(ConfigsDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    private readonly ConfigsDataAccess _dataAccess;
    [ObservableProperty] private ModelConfig? _selectedConfig;

    [RelayCommand]
    private async Task AddConfig()
    {
        var files = await this.GetTopLevel().StorageProvider.OpenFilePickerAsync(FilePickerOpenOptions);
        var file = files.Single();
        var filePath = file.Path.LocalPath;
        var name = Path.GetFileNameWithoutExtension(filePath);
        var fileContent = await File.ReadAllTextAsync(filePath);
        ModelConfig config = new(name, fileContent, ModelType.Detector);
        await _dataAccess.AddConfig(config);
        OnPropertyChanged(nameof(Configs));
    }

    [RelayCommand]
    private async Task DeleteConfig()
    {
        Guard.IsNotNull(SelectedConfig);
        await _dataAccess.RemoveConfig(SelectedConfig);
        OnPropertyChanged(nameof(Configs));
    }

    private static readonly FilePickerOpenOptions FilePickerOpenOptions = new()
    {
        Title = "Select a config file"
    };
}