using System;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface AdditionalToolingSelection : SideBarDataContext
{
	IObservable<object?> AdditionalToolingChanged { get; }
}