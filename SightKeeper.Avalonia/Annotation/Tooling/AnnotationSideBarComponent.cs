using System;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface AnnotationSideBarComponent : SideBarDataContext
{
	IObservable<object?> AdditionalToolingChanged { get; }
}