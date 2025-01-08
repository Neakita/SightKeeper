using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using FluentAssertions.Collections;
using Material.Icons;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia.Tests;

public sealed class NavigationTests
{
	[AvaloniaFact]
	public async Task ShouldShowToolTipWhenPointerOverNavigationButton()
	{
		const string tabName = "TestTab";
		TabItemViewModel tabItemViewModel = new(MaterialIconKind.Cube, tabName, new DummyViewModel());
		var window = PrepareWindow(tabItemViewModel);
		var tabItem = GetDescendantWithDataContext<TabStripItem>(window, tabItemViewModel);
		MouseMoveAtCenter(tabItem);
		await WaitToolTipShowDelay(tabItem);
		ShouldContainToolTip(window).Which.Content.Should().Be(tabName);
	}

	private static MainWindow PrepareWindow(params IReadOnlyCollection<TabItemViewModel> tabs)
	{
		MainWindow window = new()
		{
			DataContext = new MainViewModel(new DialogManager(), tabs)
		};
		window.Show();
		return window;
	}

	private static T GetDescendantWithDataContext<T>(Visual visual, object? dataContext) where T : StyledElement
	{
		return visual
			.GetVisualDescendants()
			.OfType<T>()
			.Single(item => item.DataContext == dataContext);
	}

	private static void MouseMoveAtCenter(Visual visual)
	{
		var topLevel = TopLevel.GetTopLevel(visual);
		Guard.IsNotNull(topLevel);
		var center = visual.TranslatePoint(visual.Bounds.Center, topLevel);
		Guard.IsNotNull(center);
		topLevel.MouseMove(center.Value);
	}

	private static async Task WaitToolTipShowDelay(TabStripItem tabItem)
	{
		var toolTipShowDelay = ToolTip.GetShowDelay(tabItem);
		await Task.Delay(toolTipShowDelay);
	}

	private static AndWhichConstraint<GenericCollectionAssertions<ToolTip>, ToolTip> ShouldContainToolTip(MainWindow window)
	{
		return window.GetVisualDescendants().OfType<ToolTip>().Should().ContainSingle();
	}
}