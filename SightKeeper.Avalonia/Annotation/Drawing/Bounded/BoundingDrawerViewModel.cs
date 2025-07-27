using System;
using System.Reactive.Disposables;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

public sealed partial class BoundingDrawerViewModel : ViewModel, BoundingDrawerDataContext, IDisposable
{
	public Tag? Tag
	{
		get;
		set
		{
			field = value;
			CreateItemCommand.NotifyCanExecuteChanged();
		}
	}

	ICommand BoundingDrawerDataContext.CreateItemCommand => CreateItemCommand;

	public BoundingDrawerViewModel(DataSetSelection dataSetSelection, ImageSelection imageSelection)
	{
		dataSetSelection.SelectedDataSetChanged.Subscribe(HandleDataSetSelectionChange).DisposeWith(_constructorDisposable);
		imageSelection.SelectedImageChanged.Subscribe(HandleImageSelectionChange).DisposeWith(_constructorDisposable);
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
	}

	private readonly CompositeDisposable _constructorDisposable = new();
	private AssetsOwner<ItemsMaker<AssetItem>>? _assetsLibrary;
	private Image? _image;
	private bool CanCreateItem => Tag != null && _image != null && _assetsLibrary != null;

	[RelayCommand(CanExecute = nameof(CanCreateItem))]
	private void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(Tag);
		Guard.IsNotNull(_image);
		Guard.IsNotNull(_assetsLibrary);
		var asset = _assetsLibrary.GetOrMakeAsset(_image);
		var item = asset.MakeItem(Tag);
		item.Bounding = bounding;
	}

	private void HandleDataSetSelectionChange(DataSet? set)
	{
		_assetsLibrary = set?.AssetsLibrary as AssetsOwner<ItemsMaker<AssetItem>>;
		CreateItemCommand.NotifyCanExecuteChanged();
	}

	private void HandleImageSelectionChange(Image? image)
	{
		_image = image;
		CreateItemCommand.NotifyCanExecuteChanged();
	}
}