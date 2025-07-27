using System;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Misc;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

public sealed class DeleteSelectedImageCommand : Command, IDisposable
{
	protected override bool CanExecute => _imageSelection.SelectedImage?.Assets.Count == 0;

	public DeleteSelectedImageCommand(ImageSelection imageSelection, ImageSetSelection imageSetSelection)
	{
		_imageSelection = imageSelection;
		_imageSetSelection = imageSetSelection;
		_constructorDisposable = imageSelection.SelectedImageChanged.Subscribe(_ => RaiseCanExecuteChanged());
	}

	protected override void Execute()
	{
		var set = _imageSetSelection.SelectedImageSet;
		Guard.IsNotNull(set);
		set.RemoveImageAt(_imageSelection.SelectedImageIndex);
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
	}

	private readonly ImageSelection _imageSelection;
	private readonly ImageSetSelection _imageSetSelection;
	private readonly IDisposable _constructorDisposable;
}