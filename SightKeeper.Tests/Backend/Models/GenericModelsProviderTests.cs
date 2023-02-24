using FluentAssertions;
using SightKeeper.Application.Models;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Classifier;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Tests.Backend.Models;

public sealed class GenericModelsProviderTests : DbRelatedTests
{
	[Fact]
	public void ShouldGetDetectorModel()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DetectorModel testModel = new("Test model", new Resolution());
		dbContext.DetectorModels.Add(testModel);
		dbContext.SaveChanges();

		DetectorModelsProvider.Models.Should().ContainEquivalentOf(testModel);
	}

	[Fact]
	public void ShouldGetDetectorAndClassifierModels()
	{
		
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
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


	private GenericModelsProvider<DetectorModel> DetectorModelsProvider => new(DbContextFactory);
	private GenericModelsProvider<Model> ModelsProvider => new(DbContextFactory);
}