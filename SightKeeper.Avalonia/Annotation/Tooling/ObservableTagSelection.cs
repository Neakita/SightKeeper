using System;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface ObservableTagSelection<out TTag>
{
	IObservable<TTag?> SelectedTagChanged { get; }
}