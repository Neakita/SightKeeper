using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Detector;

internal sealed class DetectorToolingViewModel(TagsContainer<Tag> tagsContainer)
	: ViewModel, DetectorToolingDataContext, TagSelection, ObservableTagSelection
{
	public IEnumerable<TagDataContext> Tags => tagsContainer.Tags.Select(tag => new TagViewModel(tag));

	public TagDataContext? SelectedTag
	{
		get;
		set
		{
			field = value;
			_selectedTagChanged.OnNext(((TagViewModel?)value)?.Tag);
		}
	}

	Tag? TagSelection.SelectedTag => ((TagViewModel?)SelectedTag)?.Tag;

	public IObservable<Tag?> SelectedTagChanged => _selectedTagChanged;

	private readonly Subject<Tag?> _selectedTagChanged = new();
}