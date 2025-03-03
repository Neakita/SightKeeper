using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class PoserToolingViewModel : ViewModel, PoserToolingDataContext, TagSelection<Tag>, ObservableTagSelection<Tag>, SelectedItemConsumer, IDisposable
{
	public TagSelectionToolingDataContext TagSelection => _tagSelection;
	public TagSelectionToolingDataContext KeyPointTagSelection => _keyPointTagSelection;

	public TagsContainer<PoserTag>? TagsSource
	{
		get;
		set
		{
			field = value;
			_tagSelection.Tags = TagsSource?.Tags ?? ReadOnlyCollection<PoserTag>.Empty;
		}
	}

	public PoserItem? SelectedItem
	{
		get;
		set
		{
			field = value;
			_keyPointTagSelection.Tags = value?.Tag.KeyPointTags ?? ReadOnlyCollection<Tag>.Empty;
			DeleteKeyPointCommand.NotifyCanExecuteChanged();
		}
	}

	BoundedItemDataContext? SelectedItemConsumer.SelectedItem
	{
		set => SelectedItem = ((PoserItemViewModel?)value)?.Value;
	}

	public Tag? SelectedTag => _tagSelection.SelectedTag ?? _keyPointTagSelection.SelectedTag;

	public IObservable<Tag?> SelectedTagChanged =>
		_tagSelection.SelectedTagChanged
			.Merge(_keyPointTagSelection.SelectedTagChanged)
			.Select(_ => SelectedTag)
			.DistinctUntilChanged();

	public PoserToolingViewModel(PoserAnnotator poserAnnotator, ObservablePoserAnnotator observablePoserAnnotator)
	{
		_poserAnnotator = poserAnnotator;
		observablePoserAnnotator.KeyPointCreated
			.Merge(observablePoserAnnotator.KeyPointDeleted)
			.Where(data => data.item == SelectedItem)
			.Subscribe(_ => DeleteKeyPointCommand.NotifyCanExecuteChanged())
			.DisposeWith(_disposable);
		MakeSureOnlyOneTagSelected();
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly TagSelectionViewModel<PoserTag> _tagSelection = new();
	private readonly TagSelectionViewModel<Tag> _keyPointTagSelection = new();
	private readonly PoserAnnotator _poserAnnotator;
	private readonly CompositeDisposable _disposable = new();

	private void MakeSureOnlyOneTagSelected()
	{
		_tagSelection.SelectedTagChanged
			.Where(tag => tag != null)
			.Subscribe(_ => _keyPointTagSelection.SelectedTag = null)
			.DisposeWith(_disposable);
		_keyPointTagSelection.SelectedTagChanged
			.Where(tag => tag != null)
			.Subscribe(_ => _tagSelection.SelectedTag = null)
			.DisposeWith(_disposable);
	}

	[RelayCommand(CanExecute = nameof(CanDeleteKeyPoint))]
	private void DeleteKeyPoint(Tag tag)
	{
		Guard.IsNotNull(SelectedItem);
		_poserAnnotator.DeleteKeyPoint(SelectedItem, tag);
	}

	private bool CanDeleteKeyPoint(Tag tag)
	{
		return SelectedItem != null && SelectedItem.KeyPoints.Any(keyPoint => keyPoint.Tag == tag);
	}
}