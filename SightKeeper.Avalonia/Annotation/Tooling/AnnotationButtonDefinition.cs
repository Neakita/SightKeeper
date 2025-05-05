using System.Windows.Input;
using Material.Icons;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed class AnnotationButtonDefinition
{
	public required MaterialIconKind IconKind { get; init; }
	public required ICommand Command { get; init; }
	public object? ToolTip { get; init; }
}