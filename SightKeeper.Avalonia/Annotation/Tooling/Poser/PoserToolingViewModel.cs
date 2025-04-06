using System;
using System.Collections.Generic;
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

	public IEnumerable<KeyPointTagDataContext> KeyPointTags
	{
		get
		{
			if (_selectedItem == null)
				return Enumerable.Empty<KeyPointTagDataContext>();
			return _selectedItem.Tag.KeyPointTags.Select(tag => new KeyPointTagViewModel(tag, new ParametrizedCommandAdapter(DeleteKeyPointCommand, tag)));
		}
	}

	[ObservableProperty] public partial TagDataContext? SelectedPoserTag { get; set; }
	[ObservableProperty] public partial TagDataContext? SelectedKeyPointTag { get; set; }

	public Tag? SelectedTag => ((TagViewModel?)SelectedPoserTag)?.Tag ?? ((TagViewModel?)SelectedKeyPointTag)?.Tag;

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
		_selectedTagChanged.OnNext(((TagViewModel?)value)?.Tag);
		if (value != null)
			SelectedKeyPointTag = null;
	}

	partial void OnSelectedKeyPointTagChanged(TagDataContext? value)
	{
		_selectedTagChanged.OnNext(((TagViewModel?)value)?.Tag);
		if (value != null)
			SelectedPoserTag = null;
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