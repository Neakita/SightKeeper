using FluentAssertions;
using SightKeeper.Backend.Models;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.Backend.Models;

public sealed class DetectorModelsServiceTests
{
	[Fact]
	public void ShouldCreateDetectorModel()
	{
		// arrange
		const string testModelName = "Test model";
		
		// act
		_service.Create(testModelName, 320, 320);
		
		// assert
		using IAppDbContext dbContext = Helper.DbContext;
		dbContext.DetectorModels.Should().Contain(model => model.Name == testModelName);
		dbContext.Clear();
	}

	[Fact]
	public void ShouldDeleteDetectorModel()
	{
		// arrange
		const string testModelName = "Test model";
		DetectorModel detectorModel = new(testModelName, new Resolution());
		using (IAppDbContext arrangeDbContext = Helper.DbContext)
		{
			arrangeDbContext.DetectorModels.Add(detectorModel);
			arrangeDbContext.SaveChanges();
		}
		
		// act
		_service.Delete(detectorModel);
		
		// assert
		using IAppDbContext dbContext = Helper.DbContext;
		dbContext.DetectorModels.Should().NotContain(model => model.Name == testModelName);
	}
	
	
	private readonly DetectorModelsService _service = new(Helper.DbProvider);
}
