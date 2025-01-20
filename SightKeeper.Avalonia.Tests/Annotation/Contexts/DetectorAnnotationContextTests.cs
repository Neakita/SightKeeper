using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Tooling;

namespace SightKeeper.Avalonia.Tests.Annotation.Contexts;

public sealed class DetectorAnnotationContextTests
{
	[Fact]
	public void ShouldHaveTagSelectionSideBar()
	{
		Context.SideBar.Should().BeOfType<TagSelectionViewModel>();
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