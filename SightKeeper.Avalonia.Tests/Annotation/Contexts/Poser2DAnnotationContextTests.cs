using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.ToolBars;

namespace SightKeeper.Avalonia.Tests.Annotation.Contexts;

public sealed class Poser2DAnnotationContextTests
{
	[Fact]
	public void ShouldHavePoserToolBar()
	{
		Context.ToolBar.Should().BeOfType<PoserToolBarViewModel>();
	}

	[Fact]
	public void ShouldHavePoser2DDrawer()
	{
		Context.Drawer.Should().BeOfType<Poser2DDrawerViewModel>();
	}

	[Fact]
	public void ShouldNotHaveDataSetByDefault()
	{
		Context.DataSet.Should().BeNull();
	}

	private static Poser2DAnnotationContext Context => new Composition().Poser2DAnnotationContext;
}