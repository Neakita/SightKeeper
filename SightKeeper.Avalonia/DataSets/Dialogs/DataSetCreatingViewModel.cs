using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.Games;
using SightKeeper.Avalonia.DataSets.Dialogs.Tag;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class DataSetCreatingViewModel : DialogViewModel<bool>, IDataSetEditorViewModel, NewDataSetInfo, INotifyDataErrorInfo, IDisposable
{
	public ViewModelValidator<NewDataSetInfo> Validator { get; }
    public IReadOnlyCollection<EditableTag> Tags => _tags;
    public IReadOnlyCollection<Game> Games => _gamesDataAccess.Games;

    public DataSetCreatingViewModel(IValidator<NewDataSetInfo> validator, GamesDataAccess gamesDataAccess)
    {
	    Validator = new ViewModelValidator<NewDataSetInfo>(validator, this, this);
        _gamesDataAccess = gamesDataAccess;
        ErrorsChanged += OnErrorsChanged;
    }

    private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ApplyCommand.NotifyCanExecuteChanged();
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddTagCommand))]
    private string _newTagName = string.Empty;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private int? _resolution = 320;
    [ObservableProperty] private Game? _game;

    private readonly ObservableCollection<EditableTag> _tags = new();
    private readonly GamesDataAccess _gamesDataAccess;

    ICommand IDataSetEditorViewModel.AddTagCommand => AddTagCommand;
    ICommand IDataSetEditorViewModel.DeleteTagCommand => DeleteTagCommand;
    ICommand IDataSetEditorViewModel.ApplyCommand => ApplyCommand;
    ICommand IDataSetEditorViewModel.CancelCommand => CancelCommand;

    [RelayCommand(CanExecute = nameof(CanAddTag))]
    private void AddTag()
    {
        _tags.Add(new NewTagViewModel(NewTagName, (byte)Tags.Count));
        NewTagName = string.Empty;
    }

    private bool CanAddTag() =>
        !string.IsNullOrWhiteSpace(NewTagName) && Tags.All(existingTag => existingTag.Name != NewTagName);

    [RelayCommand]
    private void DeleteTag(EditableTag editableTag)
    {
        Guard.IsTrue(_tags.Remove(editableTag));
    }

    [RelayCommand(CanExecute = nameof(CanApply))]
    private void Apply()
    {
	    Guard.IsFalse(HasErrors);
        Return(true);
    }

    private bool CanApply() => !HasErrors;

    [RelayCommand]
    private void Cancel()
    {
        Return(false);
    }

    IReadOnlyCollection<TagInfo> DataSetInfo.Tags =>
        Tags.Select(tag => tag.ToTagInfo()).ToList();

    public IEnumerable GetErrors(string? propertyName)
    {
	    return Validator.GetErrors(propertyName);
    }

    public bool HasErrors => Validator.HasErrors;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
	    add => Validator.ErrorsChanged += value;
	    remove => Validator.ErrorsChanged -= value;
    }

    public void Dispose()
    {
	    Validator.Dispose();
	    ErrorsChanged -= OnErrorsChanged;
    }

    public override string Header => "Create Dataset";

    protected override bool DefaultResult => false;
}