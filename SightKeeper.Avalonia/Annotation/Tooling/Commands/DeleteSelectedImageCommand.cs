using System;
using System.Reactive;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

public sealed class DeleteSelectedImageCommand : Command, IDisposable
{
	public DeleteSelectedImageCommand(ImageSelection imageSelection, ImageSetSelection imageSetSelection)
	{
		_imageSelection = imageSelection;
		_imageSetSelection = imageSetSelection;
		var assetsChanged = _imageSelection.SelectedImageChanged.Select(GetAssetsObservable).Switch();
		_constructorDisposable = imageSelection.SelectedImageChanged
			.Select(_ => Unit.Default)
			.Merge(assetsChanged)
			.Subscribe(_ => RaiseCanExecuteChanged());
	}

	protected override bool CanExecute => _imageSelection.SelectedImage?.Assets.Count == 0;

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

	private static IObservable<Unit> GetAssetsObservable(Image? image)
	{
		if (image?.Assets is IObservable<object> assetsObservable)
			return assetsObservable.Select(_ => Unit.Default);
		return Observable.Empty<Unit>();
	}
}