using FluentAssertions;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Tests.Saving.DataSets;

public sealed class DataSetSavingTests
{
	[Fact]
	public void ShouldPersistName()
	{
		var set = CreateDataSet();
		const string name = "The name";
		set.Name = name;
		var persistedSet = Persist(set);
		persistedSet.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		var set = CreateDataSet();
		const string description = "The description";
		set.Description = description;
		var persistedSet = Persist(set);
		persistedSet.Description.Should().Be(description);
	}

	private static DataSet CreateDataSet()
	{
		return new ClassifierDataSet();
	}

	private static DataSet Persist(DataSet set)
	{
		AppDataAccess appDataAccess = new();
		Utilities.AddDataSetToAppData(set, appDataAccess);
		var persistedAppData = appDataAccess.Data.Persist();
		return persistedAppData.DataSets.Single();
	}
}