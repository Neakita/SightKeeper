using System;
using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.UI.Avalonia.ViewModels.Windows;

public sealed class ModelEditorViewModel : ReactiveObject, IDisposable
{
	public static ModelEditorViewModel DesignTimeInstance => Create(new DetectorModel("Design time detector model"));

	public static ModelEditorViewModel Create(Model model) =>
		Locator.Resolve<ModelEditorViewModel, Model>(model);

	public Repository<Game> GamesRepository { get; }
	public Repository<ModelConfig> ConfigsRepository { get; }
	
	public Model Model { get; }
	
	public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
	public ReactiveCommand<Unit, Unit> CancelCommand { get; }

	public ModelEditorViewModel(
		Model model,
		AppDbContextFactory dbContextFactory,
		Repository<Game> gamesRepositoryRepository,
		Repository<ModelConfig> configsRepositoryRepository)
	{
		Model = model;
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		GamesRepository = gamesRepositoryRepository;
		ConfigsRepository = configsRepositoryRepository;
		ApplyCommand = ReactiveCommand.Create(Done);
		CancelCommand = ReactiveCommand.Create(Done);
	}

	private void Done()
	{
	}

	public void Dispose()
	{
		if (_disposed) return;
		ApplyCommand.Dispose();
		CancelCommand.Dispose();
		_disposed = true;
	}

	private bool _disposed;
}