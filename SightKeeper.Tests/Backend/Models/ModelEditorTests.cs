using FluentAssertions;
using SightKeeper.Backend;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.Backend.Models;

public sealed class ModelEditorTests : DbRelatedTests
{
	[Fact]
	public void ShouldEditorSaveChanges()
	{
		const string testModelName = "Test model";
		const string changedTestModelName = "Changed test model";
		DetectorModel testModel = new(testModelName);
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.Models.Add(testModel);
		dbContext.SaveChanges();
		IModelEditor editor = ModelEditor;
		editor.EditableModel = testModel;

		testModel.Name.Should().Be(testModelName);

		testModel.Name = changedTestModelName;

		testModel.Name.Should().Be(changedTestModelName);
		
		editor.SaveChanges();

		testModel.Name.Should().Be(changedTestModelName);

		dbContext.Models.Single(model => model.Id == testModel.Id).Should().Be(testModel);
	}

	[Fact]
	public void ShouldEditorRollbackChanges()
	{
		const string testModelName = "Test model";
		const string changedTestModelName = "Changed test model";
		DetectorModel testModel = new(testModelName);
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.Models.Add(testModel);
		dbContext.SaveChanges();
		IModelEditor editor = ModelEditor;
		editor.EditableModel = testModel;

		testModel.Name.Should().Be(testModelName);

		testModel.Name = changedTestModelName;

		testModel.Name.Should().Be(changedTestModelName);
		
		editor.DiscardChanges();

		testModel.Name.Should().Be(testModelName);

		dbContext.Models.Single(model => model.Id == testModel.Id).Should().Be(testModel);
	}


	private IModelEditor ModelEditor => new ModelEditor(DbProvider);
}
