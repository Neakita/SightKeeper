using System;

namespace SightKeeper.Avalonia.Annotation.Tooling.Adaptive;

public interface AdditionalToolingSelection
{
	IObservable<object?> AdditionalToolingChanged { get; }
}