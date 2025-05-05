using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface AnnotationButtonDefinitionsProvider
{
	IReadOnlyCollection<AnnotationButtonDefinition> ButtonDefinitions { get; }
}