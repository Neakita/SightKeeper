using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Avalonia.Annotation.ToolBars;

namespace SightKeeper.Avalonia.Tests.Annotation.Contexts;

public sealed class Poser2DAnnotationContextTests
{
	[Fact]
	public void ShouldHavePoserToolBar()
	{
		Context.ToolBar.Should().BeOfType<PoserToolBarViewModel>();
	}
	
	private static Poser2DAnnotationContext Context => new Composition().Poser2DAnnotationContext;
}