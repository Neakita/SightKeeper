using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;
using SightKeeper.Tests.Common;

namespace SightKeeper.Application.Tests.Models;

public sealed class ModelEditorTests : DbRelatedTests
{
	[Fact]
	public void ShouldEditorSaveChanges()
	{
		const string testModelName = "Test model";
		const string changedTestModelName = "Changed test model";
		DetectorModel testModel = new(testModelName);
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		dbContext.Models.Add(testModel);
		dbContext.SaveChanges();
		ModelEditor editor = ModelEditor;

		testModel.Name = changedTestModelName;

		testModel.Name.Should().Be(changedTestModelName);
		
		editor.SaveChanges(testModel);

		testModel.Name.Should().Be(changedTestModelName);

		dbContext.Models.Single(model => model.Id == testModel.Id).Should().Be(testModel);
	}

	[Fact]
	public void ShouldEditorRollbackChanges()
	{
		const string testModelName = "Test model";
		const string changedTestModelName = "Changed test model";
		DetectorModel testModel = new(testModelName);
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		dbContext.Models.Add(testModel);
		dbContext.SaveChanges();
		ModelEditor editor = ModelEditor;

		testModel.Name = changedTestModelName;

		testModel.Name.Should().Be(changedTestModelName);
		
		editor.RollbackChanges(testModel);

		testModel.Name.Should().Be(testModelName);

		dbContext.Models.Single(model => model.Id == testModel.Id).Should().Be(testModel);
	}


	private ModelEditor ModelEditor => new ModelEditorImplementation(DbContextFactory);
}
