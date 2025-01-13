using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Avalonia.Annotation.ToolBars;

namespace SightKeeper.Avalonia.Tests.Annotation.Contexts;

public sealed class ClassifierAnnotationContextTests
{
	[Fact]
	public void ShouldHaveClassifierToolBar()
	{
		Context.ToolBar.Should().BeOfType<ClassifierToolBarViewModel>();
	}

	[Fact]
	public void ShouldNotHaveDrawer()
	{
		Context.Drawer.Should().BeNull();
	}

	[Fact]
	public void ShouldNotHaveDataSetByDefault()
	{
		Context.DataSet.Should().BeNull();
	}

	private static ClassifierAnnotationContext Context => new Composition().ClassifierAnnotationContext;
}