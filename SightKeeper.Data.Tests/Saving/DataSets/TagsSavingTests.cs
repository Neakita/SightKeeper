using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.Saving.DataSets;

public sealed class TagsSavingTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		var tag = CreateTag(name);
		var persistedTag = Persist(tag);
		persistedTag.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistColor()
	{
		const uint color = 0x131517;
		var tag = CreateTag();
		tag.Color = color;
		var persistedTag = Persist(tag);
		persistedTag.Color.Should().Be(color);
	}

	private static Tag CreateTag(string name = "")
	{
		var dataSet = CreateDataSet();
		return dataSet.TagsLibrary.CreateTag(name);
	}

	private static DataSet CreateDataSet()
	{
		return new ClassifierDataSet();
	}

	private static Tag Persist(Tag tag)
	{
		var tagsLibrary = (TagsLibrary)tag.Owner;
		Guard.IsEqualTo(tagsLibrary.Tags.Count, 1);
		var dataSet = tagsLibrary.DataSet;
		var persistedDataSet = Persist(dataSet);
		return persistedDataSet.TagsLibrary.Tags.Single();
	}

	private static DataSet Persist(DataSet set)
	{
		AppDataAccess appDataAccess = new();
		Utilities.AddDataSetToAppData(set, appDataAccess);
		var persistedAppData = appDataAccess.Data.Persist();
		return persistedAppData.DataSets.Single();
	}
}