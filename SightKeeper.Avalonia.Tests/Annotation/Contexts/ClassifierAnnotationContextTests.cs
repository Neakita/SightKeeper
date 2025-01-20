using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Avalonia.Annotation.Tooling;

namespace SightKeeper.Avalonia.Tests.Annotation.Contexts;

public sealed class ClassifierAnnotationContextTests
{
	[Fact]
	public void ShouldHaveClassifierAnnotationSideBar()
	{
		Context.SideBar.Should().BeOfType<ClassifierAnnotationViewModel>();
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