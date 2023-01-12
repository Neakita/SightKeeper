using FluentAssertions;
using SightKeeper.Backend.Models;
using SightKeeper.Domain.Abstract;
using SightKeeper.Domain.Classifier;
using SightKeeper.Domain.Common;
using SightKeeper.Domain.Detector;
using SightKeeper.Persistance;

namespace SightKeeper.Tests.Backend.Models;

public sealed class GenericModelsProviderTests : DbRelatedTests
{
	[Fact]
	public void ShouldGetDetectorModel()
	{
		using AppDbContext dbContext = DbProvider.NewContext;
		DetectorModel testModel = new("Test model", new Resolution());
		dbContext.DetectorModels.Add(testModel);
		dbContext.SaveChanges();

		DetectorModelsProvider.Models.Should().ContainEquivalentOf(testModel);
	}

	[Fact]
	public void ShouldGetDetectorAndClassifierModels()
	{
		
		using AppDbContext dbContext = DbProvider.NewContext;
		DetectorModel testDetectorModel = new("Test detector model");
		ClassifierModel testClassifierModel = new("Test classifier model");
		dbContext.DetectorModels.Add(testDetectorModel);
		dbContext.ClassifierModels.Add(testClassifierModel);
		dbContext.SaveChanges();

		ModelsProvider.Models.Should()
			.ContainEquivalentOf(testDetectorModel)
			.And
			.ContainEquivalentOf(testClassifierModel);
	}


	private GenericModelsProvider<DetectorModel> DetectorModelsProvider => new(DbProvider);
	private GenericModelsProvider<Model> ModelsProvider => new(DbProvider);
}