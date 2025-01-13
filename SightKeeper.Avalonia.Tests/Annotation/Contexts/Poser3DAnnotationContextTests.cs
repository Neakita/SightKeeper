using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.ToolBars;

namespace SightKeeper.Avalonia.Tests.Annotation.Contexts;

public sealed class Poser3DAnnotationContextTests
{
	[Fact]
	public void ShouldHavePoserToolBar()
	{
		Context.ToolBar.Should().BeOfType<PoserToolBarViewModel>();
	}

	[Fact]
	public void ShouldHavePoser3DDrawer()
	{
		Context.Drawer.Should().BeOfType<Poser3DDrawerViewModel>();
	}

	[Fact]
	public void ShouldNotHaveDataSetByDefault()
	{
		Context.DataSet.Should().BeNull();
	}

	private static Poser3DAnnotationContext Context => new Composition().Poser3DAnnotationContext;
}