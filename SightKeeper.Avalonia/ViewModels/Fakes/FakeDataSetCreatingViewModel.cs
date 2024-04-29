using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeDataSetCreatingViewModel : IDataSetEditorViewModel
{
    public string Name { get; set; } = "Some data set";
    public string Description { get; set; } = "Some Description... more words and all these things";
    public int? Resolution { get; set; } = 320;

    public IReadOnlyCollection<EditableItemClass> ItemClasses { get; } = new[]
    {
        new NewItemClassViewModel("Item class 1", 0),
        new NewItemClassViewModel("Item class 2", 1),
        new NewItemClassViewModel("Item class 3", 2)
    };
    public string NewItemClassName { get; set; } = "New item class...";
    public Game? Game { get; set; }
    public IReadOnlyCollection<Game> Games { get; } = Array.Empty<Game>();
    public ICommand AddItemClassCommand { get; } = FakeCommand.Instance;
    public ICommand DeleteItemClassCommand { get; } = FakeCommand.Instance;
    public ICommand ApplyCommand { get; } = FakeCommand.Instance;
    public ICommand CancelCommand { get; } = FakeCommand.Instance;
}