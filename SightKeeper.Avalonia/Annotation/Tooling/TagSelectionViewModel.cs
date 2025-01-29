using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class TagSelectionViewModel<TTag> : ViewModel, TagSelectionToolingDataContext, TagSelection<TTag>, ObservableTagSelection<TTag>
	where TTag : Named
{
	public bool IsEnabled => true;

	[ObservableProperty] public partial IReadOnlyCollection<TTag> Tags { get; set; } = ReadOnlyCollection<TTag>.Empty;

	IReadOnlyCollection<Named> TagSelectionToolingDataContext.Tags => (IReadOnlyCollection<Named>)Tags;

	[ObservableProperty] public partial TTag? SelectedTag { get; set; }

	Named? TagSelectionToolingDataContext.SelectedTag
	{
		get => SelectedTag;
		set => SelectedTag = (TTag?)value;
	}

	public IObservable<TTag?> SelectedTagChanged => _selectedTagChanged.AsObservable();

	private readonly Subject<TTag?> _selectedTagChanged = new();

	partial void OnSelectedTagChanged(TTag? value)
	{
		_selectedTagChanged.OnNext(value);
	}
}