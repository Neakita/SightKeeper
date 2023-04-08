using System;
using System.Reactive.Linq;
using Avalonia.Metadata;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Windows;

public sealed class ModelEditorVM : ViewModel, IDisposable
{
	public static ModelEditorVM DesignTimeInstance => Create(new DetectorModel("Design time detector model"));
	
	public static ModelEditorVM Create(Model model) =>
		Locator.Resolve<ModelEditorVM, Model>(model);

	public Repository<Game> GamesRepository { get; }
	public Repository<ModelConfig> ConfigsRepository { get; }
	
	public Model Model { get; }

	[Reactive] public string NewItemClassName { get; set; } = string.Empty;

	[ObservableAsProperty] public bool CanSave { get; } = false;
	
	[Reactive] public int? SelectedItemIndex { get; set; }

	public ModelEditorVM(
		Model model,
		Repository<Game> gamesRepositoryRepository,
		Repository<ModelConfig> configsRepositoryRepository)
	{
		Model = model;
		GamesRepository = gamesRepositoryRepository;
		ConfigsRepository = configsRepositoryRepository;
		
		_disposable = this.WhenAnyValue(vm => vm.Model.Resolution.HasErrors)
			.Select(hasErrors => !hasErrors)
			.ToPropertyEx(this, vm => vm.CanSave);
	}

	public void AddNewItemClass()
	{
		ItemClass newItemClass = new(NewItemClassName);
		NewItemClassName = string.Empty;
		Model.ItemClasses.Add(newItemClass);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}
	
	private readonly IDisposable _disposable;

	[DependsOn(nameof(NewItemClassName))]
	public bool CanAddNewItemClass(object parameter) =>
		!string.IsNullOrWhiteSpace(NewItemClassName);

	public void DeleteItemClass()
	{
		if (SelectedItemIndex == null) throw new Exception();
		Model.ItemClasses.RemoveAt(SelectedItemIndex.Value);
	}

	[DependsOn(nameof(SelectedItemIndex))]
	public bool CanDeleteItemClass(object parameter) => SelectedItemIndex != null;
}