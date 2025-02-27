using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed partial class BoundingDrawerViewModel : ViewModel, BoundingDrawerDataContext
{
	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateItemCommand))]
	public partial Tag? Tag { get; set; }

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateItemCommand))]
	public partial Image? Screenshot { get; set; }

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateItemCommand))]
	public partial AssetsMaker<ItemsCreator>? AssetsLibrary { get; set; }

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

	ICommand BoundingDrawerDataContext.CreateItemCommand => CreateItemCommand;
}