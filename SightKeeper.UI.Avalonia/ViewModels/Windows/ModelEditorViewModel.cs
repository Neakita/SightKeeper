using System;
using System.Reactive;
using ReactiveUI;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Windows;

public sealed class ModelEditorViewModel : ReactiveObject, IDisposable
{
	public ModelViewModel Model { get; }
	
	public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
	public ReactiveCommand<Unit, Unit> CancelCommand { get; }

	public ModelEditorViewModel(ModelViewModel model) : this()
	{
		Model = model;
	}

	public ModelEditorViewModel()
	{
		ApplyCommand = ReactiveCommand.Create(Apply);
		CancelCommand = ReactiveCommand.Create(Cancel);
		Model = null!;
	}

	private void Apply()
	{
		// TODO save model data here
	}

	private void Cancel()
	{
		// TODO rollback model data here
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