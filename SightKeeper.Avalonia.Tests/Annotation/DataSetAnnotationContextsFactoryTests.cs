using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Avalonia.Tests.Annotation;

public sealed class DataSetAnnotationContextsFactoryTests
{
	[Fact]
	public void ShouldGetClassifierContext()
	{
		ShouldGetAnnotationContextOfType<ClassifierAnnotationContext>(new ClassifierDataSet());
	}

	[Fact]
	public void ShouldGetDetectorContext()
	{
		ShouldGetAnnotationContextOfType<DetectorAnnotationContext>(new DetectorDataSet());
	}

	[Fact]
	public void ShouldGetPoser2DContext()
	{
		ShouldGetAnnotationContextOfType<Poser2DAnnotationContext>(new Poser2DDataSet());
	}

	private static void ShouldGetAnnotationContextOfType<TContext>(DataSet dataSet) where TContext : DataSetAnnotationContext
	{
		Composition composition = new();
		DataSetAnnotationContextFactory factory = new(composition);
		var context = factory.ReuseContextOrCreateNew(null, dataSet);
		context.Should().BeOfType<TContext>();
	}
}