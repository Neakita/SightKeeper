using System;
using System.Reactive;
using Avalonia.Metadata;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.UI.Avalonia.ViewModels.Windows;

public sealed class ModelEditorViewModel : ReactiveObject, IDisposable
{
	private readonly AppDbContextFactory _dbContextFactory;
	public static ModelEditorViewModel DesignTimeInstance => Create(new DetectorModel("Design time detector model"));

	public static ModelEditorViewModel Create(Model model) =>
		Locator.Resolve<ModelEditorViewModel, Model>(model);

	public Repository<Game> GamesRepository { get; }
	public Repository<ModelConfig> ConfigsRepository { get; }
	
	public Model Model { get; }

	[Reactive] public string NewItemClassName { get; set; } = string.Empty;
	
	public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
	public ReactiveCommand<Unit, Unit> CancelCommand { get; }

	public ModelEditorViewModel(
		Model model,
		AppDbContextFactory dbContextFactory,
		Repository<Game> gamesRepositoryRepository,
		Repository<ModelConfig> configsRepositoryRepository)
	{
		_dbContextFactory = dbContextFactory;
		Model = model;
		GamesRepository = gamesRepositoryRepository;
		ConfigsRepository = configsRepositoryRepository;
		ApplyCommand = ReactiveCommand.Create(Done);
		CancelCommand = ReactiveCommand.Create(Done);
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		dbContext.Update(model);
		dbContext.Entry(model).Collection(m => m.ItemClasses).Load();
	}

	private void Done()
	{
		// TODO remove this workaround (fixes rollback will not delete added item classes)
		Model.ItemClasses = null!;
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Update(Model);
		dbContext.Entry(Model).Collection(model => model.ItemClasses).Load();
	}

	private void AddNewItemClass()
	{
		ItemClass newItemClass = new(NewItemClassName);
		NewItemClassName = string.Empty;
		Model.ItemClasses.Add(newItemClass);
	}

	[DependsOn(nameof(NewItemClassName))]
	private bool CanAddNewItemClass(object parameter) =>
		!string.IsNullOrWhiteSpace(NewItemClassName);

	private void DeleteItemClass(int selectedItemIndex) =>
		Model.ItemClasses.RemoveAt(selectedItemIndex);

	private bool CanDeleteItemClass(object parameter) =>
		parameter is int index && index != -1;

	public void Dispose()
	{
		if (_disposed) return;
		ApplyCommand.Dispose();
		CancelCommand.Dispose();
		_disposed = true;
	}

	private bool _disposed;
}