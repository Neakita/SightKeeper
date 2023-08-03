using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Extensions;

public static class ControlExtensions
{
	public static IEnumerable<Visual> GetVisualChildrenRecursive(this Visual visual)
	{
		var children = visual.GetVisualChildren();
		return children.SelectMany(child => GetVisualChildrenRecursive(child).Prepend(child));
	}

	public static TopLevel GetTopLevel(this Visual visual) =>
		TopLevel.GetTopLevel(visual) ??
		ThrowHelper.ThrowArgumentException<TopLevel>(nameof(visual), "Could`t find TopLevel of the visual");
}