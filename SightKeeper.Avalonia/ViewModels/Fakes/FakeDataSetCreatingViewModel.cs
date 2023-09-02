using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using NSubstitute;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeDataSetCreatingViewModel : IDataSetEditorViewModel
{
    public FakeDataSetCreatingViewModel()
    {
        Name = "Some data set";
    }

    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;
    public string Name { get; set; }
    public string Description { get; set; } = "Some Description... more words and all these things";
    public int? Resolution { get; set; } = 320;
    public IReadOnlyCollection<string> ItemClasses { get; } = new []{ "Item class 1", "Item class 2" };
    public string? SelectedItemClass { get; set; }
    public string NewItemClassName { get; set; }
    public Game? Game { get; set; }
    public Task<IReadOnlyCollection<Game>> Games { get; } = Task.FromResult((IReadOnlyCollection<Game>)Array.Empty<Game>());
    public ICommand AddItemClassCommand { get; } = Substitute.For<ICommand>();
    public ICommand DeleteItemClassCommand { get; } = Substitute.For<ICommand>();
    public ICommand ApplyCommand { get; } = Substitute.For<ICommand>();
    public ICommand CancelCommand { get; } = Substitute.For<ICommand>();
}