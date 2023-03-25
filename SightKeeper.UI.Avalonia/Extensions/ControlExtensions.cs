using System;
using Avalonia.Controls;

namespace SightKeeper.UI.Avalonia.Extensions;

public static class ControlExtensions
{
	public static Window GetParentWindow(this IControl control)
	{
		IControl? parent = control.Parent;
		if (parent == null) throw new Exception("Parent window not found");
		return parent as Window ?? GetParentWindow(parent);
	}
}