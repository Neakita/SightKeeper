using System;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface AnnotationImageDataContext : ImageDataContext
{
	DateTimeOffset CreationTimestamp { get; }
	bool IsAsset { get; }
}