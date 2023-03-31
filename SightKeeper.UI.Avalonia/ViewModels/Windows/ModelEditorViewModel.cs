using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Infrastructure.Common;
using SightKeeper.Infrastructure.Data;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Windows;

public sealed class ModelEditorViewModel : ReactiveObject, IDisposable
{
	public static ModelEditorViewModel Create(ModelVM model) =>
		Locator.Resolve<ModelEditorViewModel, ModelVM>(model);

	public IReadOnlyCollection<Game> Games { get; }
	public IReadOnlyCollection<ModelConfig> Configs { get; }

	public ModelVM Model { get; }
	
	public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
	public ReactiveCommand<Unit, Unit> CancelCommand { get; }

	public ModelEditorViewModel(ModelVM model, AppDbContextFactory dbContextFactory) : this()
	{
		Model = model;
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		Games = dbContext.Games.ToList();
		Configs = dbContext.ModelConfigs.ToList(); // TODO change to use static repository
	}

	public ModelEditorViewModel()
	{
		ApplyCommand = ReactiveCommand.Create(Done);
		CancelCommand = ReactiveCommand.Create(Done);
		Model = null!;
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