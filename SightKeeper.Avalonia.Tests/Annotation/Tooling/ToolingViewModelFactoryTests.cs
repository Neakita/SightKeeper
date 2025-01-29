using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling;

public sealed class ToolingViewModelFactoryTests
{
	[Fact]
	public void ShouldReturnClassifierAnnotation()
	{
		var factory = CreateFactory();
		var tooling = factory.CreateToolingViewModel(new ClassifierDataSet());
		tooling.Should().BeOfType<ClassifierAnnotationViewModel>();
	}

	[Fact]
	public void ShouldReturnTagSelectionForDetectorDataSet()
	{
		var factory = CreateFactory();
		var tooling = factory.CreateToolingViewModel(new DetectorDataSet());
		tooling.Should().BeOfType<TagSelectionViewModel<Tag>>();
	}

	[Fact]
	public void ShouldReturnPoserToolingForPoser2DDataSet()
	{
		var factory = CreateFactory();
		var tooling = factory.CreateToolingViewModel(new Poser2DDataSet());
		tooling.Should().BeOfType<PoserToolingViewModel>();
	}

	private static ToolingViewModelFactory CreateFactory()
	{
		return new ToolingViewModelFactory(new Composition());
	}
}