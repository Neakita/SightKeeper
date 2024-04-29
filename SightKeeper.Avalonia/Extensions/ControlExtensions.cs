using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Extensions;

public static class ControlExtensions
{
	public static TopLevel GetTopLevel(this Visual visual) =>
		TopLevel.GetTopLevel(visual) ??
		ThrowHelper.ThrowArgumentException<TopLevel>(nameof(visual), "Could`t find TopLevel of the visual");
}