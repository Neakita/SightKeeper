using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Material.Icons;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.Games;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.MessageBoxDialog;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.Tag;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;

internal sealed partial class DataSetEditingViewModel : DialogViewModel<bool>, INotifyDataErrorInfo, IDataSetEditorViewModel, DataSetChanges, IDisposable
{
	public ViewModelValidator<DataSetChanges> Validator { get; }
    IReadOnlyCollection<TagInfo> DataSetInfo.Tags =>
        _tags.Select(tag => tag.ToTagInfo()).ToList();
    public IReadOnlyCollection<EditableTag> Tags => _tags;
    public IReadOnlyCollection<Game> Games => _gamesDataAccess.Games;
    public DetectorDataSet DataSet { get; private set; }

    public DataSetEditingViewModel(
	    DetectorDataSet dataSet,
	    IValidator<DataSetChanges> validator,
	    GamesDataAccess gamesDataAccess,
	    DialogManager dialogManager)
    {
	    Validator = new ViewModelValidator<DataSetChanges>(validator, this, this);
        _gamesDataAccess = gamesDataAccess;
        _dialogManager = dialogManager;
        SetData(dataSet);
        ErrorsChanged += OnErrorsChanged;
    }

    private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
	    ApplyCommand.NotifyCanExecuteChanged();
    }

    [MemberNotNull(nameof(DataSet))]
    private void SetData(DetectorDataSet dataSet)
    {
        DataSet = dataSet;
        _tags.Clear();
        foreach (var tag in dataSet.Tags)
            _tags.Add(new ExistingTag(tag));
        Name = dataSet.Name;
        Description = dataSet.Description;
        Resolution = dataSet.Resolution;
        Game = dataSet.Game;
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddTagCommand))]
    private string _newTagName = string.Empty;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private int? _resolution = 320;
    [ObservableProperty] private Game? _game;

    private readonly ObservableCollection<EditableTag> _tags = new();
    private readonly List<DeletedTag> _deletedTags = new();
    private readonly GamesDataAccess _gamesDataAccess;
    private readonly DialogManager _dialogManager;

    ICommand IDataSetEditorViewModel.AddTagCommand => AddTagCommand;
    [RelayCommand(CanExecute = nameof(CanAddTag))]
    private async Task AddTag()
    {
        var deletedTagWithTheSameName = _deletedTags.FirstOrDefault(deletedTag => deletedTag.Tag.Name == NewTagName);
        if (deletedTagWithTheSameName != null)
        {
            var noButton = new MessageBoxButtonDefinition("No", MaterialIconKind.Close);
            MessageBoxDialogViewModel messageBox = new(
                "Item class adding",
                $"Item class {NewTagName} already exists, but it was deleted. Would you like to re-add it?",
                new MessageBoxButtonDefinition("Yes", MaterialIconKind.Check),
                noButton);
            var result = await _dialogManager.ShowDialogAsync(messageBox);
            if (result == noButton)
                return;
            var isRemoved = _deletedTags.Remove(deletedTagWithTheSameName);
            Guard.IsTrue(isRemoved);
            _tags.Add(new ExistingTag(deletedTagWithTheSameName.Tag));
        }
        else
        {
            _tags.Add(new NewTagViewModel(NewTagName, (byte)_tags.Count));
        }
        NewTagName = string.Empty;
    }

    private bool CanAddTag() =>
        !string.IsNullOrWhiteSpace(NewTagName) &&
        Tags.All(existingTag => existingTag.Name != NewTagName) &&
        _tags.Count < 80;

    ICommand IDataSetEditorViewModel.DeleteTagCommand => DeleteTagCommand;
    [RelayCommand]
    private Task DeleteTag(EditableTag tag)
    {
	    throw new NotImplementedException();
	    // ISSUE HUGE mess!
	    // if (tag is ExistingTag existingTag)
	    // {
	    //     DeletedTagAction? action = null;
	    //     if (_objectsLookupper.GetItems(existingTag.Tag).Any())
	    //     {
	    //         var cancel = new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel);
	    //         var deleteItems = new MessageBoxButtonDefinition("Delete items", MaterialIconKind.TrashCanOutline);
	    //         var deleteAssets = new MessageBoxButtonDefinition("Delete assets", MaterialIconKind.TrashCanOutline);
	    //         var deleteScreenshots = new MessageBoxButtonDefinition("Delete screenshots", MaterialIconKind.TrashCanOutline);
	    //         MessageBoxDialogViewModel messageBox = new(
	    //             "Associated items",
	    //             $"Item class {tag.Name} has some items ({_objectsLookupper.GetItems(existingTag.Tag).Count}). Are you sure you want to delete it? Choose action",
	    //             cancel, deleteItems, deleteAssets, deleteScreenshots);
	    //         var result = await _dialogManager.ShowDialogAsync(messageBox);
	    //         if (result == cancel)
	    //             return;
	    //         if (result == deleteItems)
	    //             action = DeletedTagAction.DeleteItems;
	    //         else if (result == deleteAssets)
	    //             action = DeletedTagAction.DeleteAssets;
	    //         else if (result == deleteScreenshots)
	    //             action = DeletedTagAction.DeleteScreenshots;
	    //     }
	    //     _deletedTags.Add(new DeletedTag(existingTag.Tag, action));
	    // }
	    // Guard.IsTrue(_tags.Remove(tag));
    }

    ICommand IDataSetEditorViewModel.ApplyCommand => ApplyCommand;
    [RelayCommand(CanExecute = nameof(CanApply))]
    private void Apply()
    {
        Guard.IsFalse(HasErrors);
        Return(true);
    }

    private bool CanApply() => !HasErrors;

    ICommand IDataSetEditorViewModel.CancelCommand => CancelCommand;
    [RelayCommand]
    private void Cancel() => Return(false);

    public void Dispose()
    {
	    ErrorsChanged -= OnErrorsChanged;
    }

    IReadOnlyCollection<TagInfo> DataSetChanges.NewTags => Tags
        .OfType<NewTagViewModel>()
        .Select(newTag => newTag.ToTagInfo())
        .ToList();
    IReadOnlyCollection<EditedTag> DataSetChanges.EditedTags => Tags
        .OfType<ExistingTag>()
        .Where(existingTag => existingTag.IsEdited)
        .Select(existingTag => existingTag.ToEditedTag())
        .ToList();
    IReadOnlyCollection<DeletedTag> DataSetChanges.DeletedTags => _deletedTags;

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

    public override string Header => "Edit Dataset";

    protected override bool DefaultResult => false;
}