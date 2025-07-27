using System;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
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

	public Image? Image
	{
		get;
		set
		{
			field = value;
			CreateItemCommand.NotifyCanExecuteChanged();
		}
	}

	ICommand BoundingDrawerDataContext.CreateItemCommand => CreateItemCommand;

	public BoundingDrawerViewModel(DataSetSelection dataSetSelection)
	{
		_disposable = dataSetSelection.SelectedDataSetChanged.Subscribe(HandleDataSetSelectionChange);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly IDisposable _disposable;
	private AssetsOwner<ItemsMaker<AssetItem>>? _assetsLibrary;
	private bool CanCreateItem => Tag != null && Image != null && _assetsLibrary != null;

	[RelayCommand(CanExecute = nameof(CanCreateItem))]
	private void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(Tag);
		Guard.IsNotNull(Image);
		Guard.IsNotNull(_assetsLibrary);
		var asset = _assetsLibrary.GetOrMakeAsset(Image);
		var item = asset.MakeItem(Tag);
		item.Bounding = bounding;
	}

	private void HandleDataSetSelectionChange(DataSet? set)
	{
		_assetsLibrary = set?.AssetsLibrary as AssetsOwner<ItemsMaker<AssetItem>>;
		CreateItemCommand.NotifyCanExecuteChanged();
	}
}