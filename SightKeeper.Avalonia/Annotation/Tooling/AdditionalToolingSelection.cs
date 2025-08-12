using System;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface AdditionalToolingSelection
{
	IObservable<object?> AdditionalToolingChanged { get; }
}