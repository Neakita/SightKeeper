using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

public sealed partial class PoserToolingViewModel : ViewModel, PoserToolingDataContext, TagSelection, ObservableTagSelection, SelectedItemConsumer, IDisposable
{
	public TagsContainer<PoserTag>? TagsSource
	{
		get;
		set
		{
			OnPropertyChanging(nameof(PoserTags));
			field = value;
			OnPropertyChanged(nameof(PoserTags));
		}
	}

	public BoundedItemDataContext? SelectedItem
	{
		set
		{
			OnPropertyChanging(nameof(KeyPointTags));
			_selectedItem = ((PoserItemViewModel?)value)?.Value;
			KeyPointTags = _selectedItem == null ? ReadOnlyCollection<KeyPointTagDataContext>.Empty : _selectedItem.Tag.KeyPointTags.Select(tag => new KeyPointTagViewModel(tag, new ParametrizedCommandAdapter(DeleteKeyPointCommand, tag))).ToList();
			OnPropertyChanged(nameof(KeyPointTags));
		}
	}

	public IEnumerable<TagDataContext> PoserTags
	{
		get
		{
			if (TagsSource == null)
				return Enumerable.Empty<TagDataContext>();
			return TagsSource.Tags.Select(tag => new TagViewModel(tag));
		}
	}

	[ObservableProperty]
	public partial IReadOnlyCollection<KeyPointTagDataContext> KeyPointTags { get; private set; } =
		ReadOnlyCollection<KeyPointTagDataContext>.Empty;

	[ObservableProperty] public partial TagDataContext? SelectedPoserTag { get; set; }
	[ObservableProperty] public partial KeyPointTagViewModel? SelectedKeyPointTag { get; set; }

	TagDataContext? PoserToolingDataContext.SelectedKeyPointTag
	{
		get => SelectedKeyPointTag;
		set => SelectedKeyPointTag = (KeyPointTagViewModel?)value;
	}

	public Tag? SelectedTag => ((TagViewModel?)SelectedPoserTag)?.Tag ?? SelectedKeyPointTag?.Tag;

	public IObservable<Tag?> SelectedTagChanged => _selectedTagChanged.DistinctUntilChanged();

	public PoserToolingViewModel(PoserAnnotator poserAnnotator, ObservablePoserAnnotator observablePoserAnnotator)
	{
		_poserAnnotator = poserAnnotator;
		observablePoserAnnotator.KeyPointCreated
			.Merge(observablePoserAnnotator.KeyPointDeleted)
			.Where(data => data.item == _selectedItem)
			.Subscribe(_ => DeleteKeyPointCommand.NotifyCanExecuteChanged())
			.DisposeWith(_disposable);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly PoserAnnotator _poserAnnotator;
	private readonly CompositeDisposable _disposable = new();
	private readonly Subject<Tag?> _selectedTagChanged = new();
	private PoserItem? _selectedItem;

	partial void OnSelectedPoserTagChanged(TagDataContext? value)
	{
		if (value != null)
			SelectedKeyPointTag = null;
		_selectedTagChanged.OnNext(((TagViewModel?)value)?.Tag);
	}

	partial void OnSelectedKeyPointTagChanged(KeyPointTagViewModel? value)
	{
		if (value != null)
			SelectedPoserTag = null;
		var tag = value?.Tag;
		_selectedTagChanged.OnNext(tag);
	}

	[RelayCommand(CanExecute = nameof(CanDeleteKeyPoint))]
	private void DeleteKeyPoint(Tag tag)
	{
		Guard.IsNotNull(_selectedItem);
		_poserAnnotator.DeleteKeyPoint(_selectedItem, tag);
	}

	private bool CanDeleteKeyPoint(Tag tag)
	{
		return _selectedItem != null && _selectedItem.KeyPoints.Any(keyPoint => keyPoint.Tag == tag);
	}
}