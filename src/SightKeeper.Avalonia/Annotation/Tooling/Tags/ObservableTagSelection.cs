using System;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Tags;

public interface ObservableTagSelection
{
	IObservable<Tag?> SelectedTagChanged { get; }
}