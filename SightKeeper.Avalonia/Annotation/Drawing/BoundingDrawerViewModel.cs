using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed partial class BoundingDrawerViewModel : ViewModel
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

	public Screenshot? Screenshot
	{
		get;
		set
		{
			field = value;
			CreateItemCommand.NotifyCanExecuteChanged();
		}
	}

	public AssetsMaker<ItemsCreator>? AssetsLibrary
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
	private bool CanCreateItem => Tag != null && Screenshot != null && AssetsLibrary != null;

	[RelayCommand(CanExecute = nameof(CanCreateItem))]
	private void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(Tag);
		Guard.IsNotNull(Screenshot);
		Guard.IsNotNull(AssetsLibrary);
		_annotator.CreateItem(AssetsLibrary, Screenshot, Tag, bounding);
	}
}