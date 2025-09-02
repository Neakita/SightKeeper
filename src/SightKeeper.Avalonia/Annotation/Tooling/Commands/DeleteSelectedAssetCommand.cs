using System;
using System.Reactive;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

public sealed class DeleteSelectedAssetCommand : Command, IDisposable
{
	protected override bool CanExecute
	{
		get
		{
			var image = _imageSelection.SelectedImage;
			var dataSet = _dataSetSelection.SelectedDataSet;
			return image != null && dataSet != null && dataSet.AssetsLibrary.Contains(image);
		}
	}

	public DeleteSelectedAssetCommand(ImageSelection imageSelection, DataSetSelection dataSetSelection)
	{
		_imageSelection = imageSelection;
		_dataSetSelection = dataSetSelection;
		var selectedImageChanged = _imageSelection.SelectedImageChanged.Select(_ => Unit.Default);
		var assetsChanged = _imageSelection.SelectedImageChanged.Select(GetAssetsObservable).Switch();
		var selectedDataSetChanged = _dataSetSelection.SelectedDataSetChanged
			.Select(_ => Unit.Default);
		_constructorDisposable =
			selectedImageChanged
			.Merge(assetsChanged)
			.Merge(selectedDataSetChanged)
			.Subscribe(_ => RaiseCanExecuteChanged());
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
	}

	protected override void Execute()
	{
		var image = _imageSelection.SelectedImage;
		Guard.IsNotNull(image);
		var dataSet = _dataSetSelection.SelectedDataSet;
		Guard.IsNotNull(dataSet);
		dataSet.AssetsLibrary.DeleteAsset(image);
	}

	private readonly ImageSelection _imageSelection;
	private readonly DataSetSelection _dataSetSelection;
	private readonly IDisposable _constructorDisposable;

	private static IObservable<Unit> GetAssetsObservable(ManagedImage? image)
	{
		if (image?.Assets is IObservable<object> assetsObservable)
			return assetsObservable.Select(_ => Unit.Default);
		return Observable.Empty<Unit>();
	}
}