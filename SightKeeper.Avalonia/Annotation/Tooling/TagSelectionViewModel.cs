using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class TagSelectionViewModel : ViewModel, TagSelectionToolingDataContext, TagSelection, ObservableTagSelection
{
	public bool IsEnabled => true;

	[ObservableProperty] public partial IReadOnlyCollection<Tag> Tags { get; set; } = ReadOnlyCollection<Tag>.Empty;

	IReadOnlyCollection<Named> TagSelectionToolingDataContext.Tags => Tags;

	[ObservableProperty] public partial Tag? SelectedTag { get; set; }

	Named? TagSelectionToolingDataContext.SelectedTag
	{
		get => SelectedTag;
		set => SelectedTag = (Tag?)value;
	}

	public IObservable<Tag?> SelectedTagChanged => _selectedTagChanged.AsObservable();

	private readonly Subject<Tag?> _selectedTagChanged = new();

	partial void OnSelectedTagChanged(Tag? value)
	{
		_selectedTagChanged.OnNext(value);
	}
}