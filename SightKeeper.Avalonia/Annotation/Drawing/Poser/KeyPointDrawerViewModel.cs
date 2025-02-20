using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed partial class KeyPointDrawerViewModel : ViewModel, KeyPointDrawerDataContext
{
	[ObservableProperty, NotifyPropertyChangedFor(nameof(ExistingKeyPoint)), NotifyCanExecuteChangedFor(nameof(CreateKeyPointCommand))]
	public partial Tag? Tag { get; set; }

	[ObservableProperty, NotifyPropertyChangedFor(nameof(ExistingKeyPoint)), NotifyCanExecuteChangedFor(nameof(CreateKeyPointCommand))]
	public partial PoserItemViewModel? Item { get; set; }

	public KeyPointViewModel? ExistingKeyPoint => Item?.KeyPoints.SingleOrDefault(keyPoint => keyPoint.Tag == Tag);

	public KeyPointDrawerViewModel(PoserAnnotator annotator)
	{
		_annotator = annotator;
	}

	private readonly PoserAnnotator _annotator;
	private bool CanCreateItem => Tag != null && Item != null;

	[RelayCommand(CanExecute = nameof(CanCreateItem))]
	private void CreateKeyPoint(Vector2<double> position)
	{
		Guard.IsNotNull(Tag);
		Guard.IsNotNull(Item);
		_annotator.CreateKeyPoint(Item.Value, Tag, position);
	}

	ICommand KeyPointDrawerDataContext.CreateKeyPointCommand => CreateKeyPointCommand;
}