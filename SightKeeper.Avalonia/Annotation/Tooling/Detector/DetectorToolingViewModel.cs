using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Detector;

internal sealed class DetectorToolingViewModel : ViewModel, DetectorToolingDataContext, TagSelection, ObservableTagSelection
{
	public TagsContainer<DomainTag>? TagsContainer
	{
		get;
		set
		{
			OnPropertyChanging(nameof(Tags));
			field = value;
			OnPropertyChanged(nameof(Tags));
		}
	}

	public IEnumerable<TagDataContext> Tags
	{
		get
		{
			if (TagsContainer == null)
				return Enumerable.Empty<TagDataContext>();
			return TagsContainer.Tags.Select(tag => new TagViewModel(tag));
		}
	}

	public TagDataContext? SelectedTag
	{
		get;
		set
		{
			field = value;
			_selectedTagChanged.OnNext(((TagViewModel?)value)?.Tag);
		}
	}

	DomainTag? TagSelection.SelectedTag => ((TagViewModel?)SelectedTag)?.Tag;

	public IObservable<DomainTag?> SelectedTagChanged => _selectedTagChanged;

	private readonly Subject<DomainTag?> _selectedTagChanged = new();
}