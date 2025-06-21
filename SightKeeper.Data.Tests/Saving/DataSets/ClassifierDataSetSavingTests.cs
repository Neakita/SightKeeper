using FluentAssertions;

namespace SightKeeper.Data.Tests.Saving.DataSets;

public sealed class ClassifierDataSetSavingTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		var set = Utilities.CreateClassifierDataSet();
		set.Name = name;
		var persistedSet = set.Persist();
		persistedSet.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		const string description = "The description";
		var set = Utilities.CreateClassifierDataSet();
		set.Description = description;
		var persistedSet = set.Persist();
		persistedSet.Description.Should().Be(description);
	}
}