using System;
using System.Reactive.Linq;
using SightKeeper.Avalonia.Annotation.Tooling.Adaptive;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Tags;

public sealed class TagSelectionProvider(AdditionalToolingSelection additionalToolingSelection)
{
	public IObservable<Tag?> SelectedTagChanged => additionalToolingSelection.AdditionalToolingChanged
		.Select(ToolingAsObservableTag)
		.Switch()
		.DistinctUntilChanged();

	private static IObservable<Tag?> ToolingAsObservableTag(object? tooling)
	{
		var tagObservable = Observable.Empty<Tag?>();
		if (tooling is ObservableTagSelection observableTagSelection)
			tagObservable = observableTagSelection.SelectedTagChanged;
		if (tooling is TagSelection tagSelection)
			tagObservable = tagObservable.Prepend(tagSelection.SelectedTag);
		return tagObservable;
	}
}