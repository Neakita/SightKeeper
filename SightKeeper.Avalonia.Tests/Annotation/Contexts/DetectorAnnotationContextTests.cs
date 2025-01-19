using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.ToolBars;

namespace SightKeeper.Avalonia.Tests.Annotation.Contexts;

public sealed class DetectorAnnotationContextTests
{
	[Fact]
	public void ShouldHaveDetectorToolBar()
	{
		Context.Annotation.Should().BeOfType<DetectorToolBarViewModel>();
	}

	[Fact]
	public void ShouldHaveDetectorDrawer()
	{
		Context.Drawer.Should().BeOfType<DetectorDrawerViewModel>();
	}

	[Fact]
	public void ShouldNotHaveDataSetByDefault()
	{
		Context.DataSet.Should().BeNull();
	}

	private static DetectorAnnotationContext Context => new Composition().DetectorAnnotationContext;
}