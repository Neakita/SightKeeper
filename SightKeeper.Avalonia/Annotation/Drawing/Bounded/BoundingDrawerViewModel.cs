using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

public sealed partial class BoundingDrawerViewModel : ViewModel, BoundingDrawerDataContext
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

	public AssetsOwner<ItemsMaker<AssetItem>>? AssetsLibrary
	{
		get;
		set
		{
			field = value;
			CreateItemCommand.NotifyCanExecuteChanged();
		}
	}

	private bool CanCreateItem => Tag != null && Image != null && AssetsLibrary != null;

	[RelayCommand(CanExecute = nameof(CanCreateItem))]
	private void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(Tag);
		Guard.IsNotNull(Image);
		Guard.IsNotNull(AssetsLibrary);
		var asset = AssetsLibrary.GetOrMakeAsset(Image);
		var item = asset.MakeItem(Tag);
		item.Bounding = bounding;
	}

	ICommand BoundingDrawerDataContext.CreateItemCommand => CreateItemCommand;
}