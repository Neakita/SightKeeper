using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

public sealed partial class PoserToolingViewModel :
	ViewModel,
	PoserToolingDataContext,
	TagSelection,
	ObservableTagSelection,
	IDisposable
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

	public PoserToolingViewModel(SelectedItemProvider selectedItemProvider)
	{
		_constructorDisposable = selectedItemProvider.SelectedItemChanged.Subscribe(HandleItemSelectionChange);
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
		_selectedTagChanged.Dispose();
	}

	private readonly IDisposable _constructorDisposable;
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
		var keyPoint = _selectedItem.KeyPoints.Single(keyPoint => keyPoint.Tag == tag);
		_selectedItem.DeleteKeyPoint(keyPoint);
	}

	private bool CanDeleteKeyPoint(Tag tag)
	{
		return _selectedItem != null && _selectedItem.KeyPoints.Any(keyPoint => keyPoint.Tag == tag);
	}

	private void HandleItemSelectionChange(DetectorItem? item)
	{
		if (item is PoserItem poserItem)
		{
			_selectedItem = poserItem;
			KeyPointTags = _selectedItem.Tag.KeyPointTags.Select(TagToViewModel).ToList();
		}
		else
		{
			_selectedItem = null;
			KeyPointTags = ReadOnlyCollection<KeyPointTagDataContext>.Empty;
		}

		OnPropertyChanged(nameof(KeyPointTags));
	}

	private KeyPointTagViewModel TagToViewModel(Tag tag)
	{
		var parametrizedDeleteKeyPointCommand = new ParametrizedCommandAdapter(DeleteKeyPointCommand, tag);
		return new KeyPointTagViewModel(tag, parametrizedDeleteKeyPointCommand);
	}
}