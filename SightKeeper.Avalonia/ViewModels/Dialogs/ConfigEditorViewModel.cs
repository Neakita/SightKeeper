using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application.Config;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public partial class ConfigEditorViewModel : ValidatableViewModel<ConfigData>, ConfigData, DialogViewModel
{
    public static ModelType[] ModelTypes { get; } = { ModelType.Detector };
    
    public IObservable<Unit> CloseRequested => _closeRequested;
    
    public bool? DialogResult { get; private set; }

    public void SetValues(ModelConfig config)
    {
        Name = config.Name;
        Content = config.Content;
        ModelType = config.ModelType;
    }
    
    public ConfigEditorViewModel(IValidator<ConfigData> validator) : base(validator)
    {
        Name = "New config";
    }

    private readonly Subject<Unit> _closeRequested = new();
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private string _name = string.Empty;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private byte[] _content = Array.Empty<byte>();
    [ObservableProperty] private ModelType _modelType = ModelType.Detector;

    [RelayCommand]
    private async Task SelectFromFile(CancellationToken cancellationToken)
    {
        var files = await this.GetTopLevel().StorageProvider.OpenFilePickerAsync(FilePickerOptions);
        var file = files.SingleOrDefault();
        if (file == null) return;
        var filePath = file.Path.LocalPath;
        Name = Path.GetFileNameWithoutExtension(filePath);
        Content = await File.ReadAllBytesAsync(filePath, cancellationToken);
    }

    [RelayCommand(CanExecute = nameof(CanApply))]
    private void Apply()
    {
        DialogResult = true;
        _closeRequested.OnNext(Unit.Default);
    }

    private bool CanApply() =>
        ValidationResult.IsValid;

    [RelayCommand]
    private void Cancel()
    {
        DialogResult = false;
        _closeRequested.OnNext(Unit.Default);
    }

    private static readonly FilePickerOpenOptions FilePickerOptions = new()
    {
        Title = "Select a config file", FileTypeFilter = new[]
        {
            new FilePickerFileType("All")
            {
                Patterns = new[] { "*.*" }
            },
            new FilePickerFileType("Config")
            {
                Patterns = new[] { "*.cfg", "*.config" }
            }
        }
    };
}