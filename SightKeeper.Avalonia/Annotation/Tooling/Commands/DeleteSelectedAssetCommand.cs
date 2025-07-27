using System;
using System.Reactive;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Misc;

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
		_constructorDisposable = _dataSetSelection.SelectedDataSetChanged
			.Select(_ => Unit.Default)
			.Merge(selectedImageChanged)
			.Subscribe(_ => RaiseCanExecuteChanged());
	}

	protected override void Execute()
	{
		var image = _imageSelection.SelectedImage;
		Guard.IsNotNull(image);
		var dataSet = _dataSetSelection.SelectedDataSet;
		Guard.IsNotNull(dataSet);
		dataSet.AssetsLibrary.DeleteAsset(image);
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
	}

	private readonly ImageSelection _imageSelection;
	private readonly DataSetSelection _dataSetSelection;
	private readonly IDisposable _constructorDisposable;
}