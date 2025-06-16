using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

public sealed partial class BoundingDrawerViewModel : ViewModel, BoundingDrawerDataContext
{
	public DomainTag? Tag
	{
		get;
		set
		{
			field = value;
			CreateItemCommand.NotifyCanExecuteChanged();
		}
	}

	public DomainImage? Image
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

	public BoundingDrawerViewModel(BoundingAnnotator annotator)
	{
		_annotator = annotator;
	}

	private readonly BoundingAnnotator _annotator;
	private bool CanCreateItem => Tag != null && Image != null && AssetsLibrary != null;

	[RelayCommand(CanExecute = nameof(CanCreateItem))]
	private void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(Tag);
		Guard.IsNotNull(Image);
		Guard.IsNotNull(AssetsLibrary);
		_annotator.CreateItem(AssetsLibrary, Image, Tag, bounding);
	}

	ICommand BoundingDrawerDataContext.CreateItemCommand => CreateItemCommand;
}