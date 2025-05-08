using System;
using System.Reactive;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

internal sealed class DeleteSelectedAssetButtonDefinitionFactory : AnnotationButtonDefinitionFactory
{
	public required ImageSelection ImageSelection { private get; init; }
	public required DataSetSelection DataSetSelection { private get; init; }
	public required ObservableAnnotator ObservableAnnotator { private get; init; }
	public required AssetDeleter AssetDeleter { private get; init; }

	public AnnotationButtonDefinition CreateButtonDefinition()
	{
		return new AnnotationButtonDefinition
		{
			IconKind = MaterialIconKind.BookmarkRemove,
			Command = CreateCommand(),
			ToolTip = "Delete selected asset"
		};
	}

	private bool CanDeleteSelectedAsset
	{
		get
		{
			var image = ImageSelection.SelectedImage;
			var dataSet = DataSetSelection.SelectedDataSet;
			return image != null && dataSet != null && dataSet.AssetsLibrary.Contains(image);
		}
	}

	private DisposableCommand CreateCommand()
	{
		RelayCommand command = new(DeleteSelectedAsset, () => CanDeleteSelectedAsset);
		var selectedImageChanged = ImageSelection.SelectedImageChanged.Select(_ => Unit.Default);
		var selectedImageAssetsChanged =
			ObservableAnnotator.AssetsChanged
				.Where(image => image == ImageSelection.SelectedImage)
				.Select(_ => Unit.Default);
		var canExecuteSubscription = DataSetSelection.SelectedDataSetChanged
			.Select(_ => Unit.Default)
			.Merge(selectedImageChanged)
			.Merge(selectedImageAssetsChanged)
			.Subscribe(_ => command.NotifyCanExecuteChanged());
		return new DisposableCommand(command, canExecuteSubscription);
	}

	private void DeleteSelectedAsset()
	{
		var image = ImageSelection.SelectedImage;
		Guard.IsNotNull(image);
		var dataSet = DataSetSelection.SelectedDataSet;
		Guard.IsNotNull(dataSet);
		AssetDeleter.DeleteAsset(dataSet.AssetsLibrary, image);
	}
}