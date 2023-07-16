using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.VisualTree;

namespace SightKeeper.Avalonia.Extensions;

public static class ControlExtensions
{
	public static IEnumerable<Visual> GetVisualChildrenRecursive(this Visual visual)
	{
		var children = visual.GetVisualChildren();
		return children.SelectMany(child => GetVisualChildrenRecursive(child).Prepend(child));
	}
}