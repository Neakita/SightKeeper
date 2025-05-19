using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class DataSetEditorTests
{
	[Fact]
	public void ShouldChangeDataSetProperties()
	{
		var dataSet = CreateDataSet();
		DataSetEditor editor = new();
		const string name = "new name";
		const string description = "new description";
		var data = Utilities.CreateDataSetData(name, description);
		editor.Edit(dataSet, data);
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldNotifyObserver()
	{
		var dataSet = CreateDataSet();
		DataSetEditor editor = new();
		var data = Utilities.CreateDataSetData("new name", "new description");
		var observer = Substitute.For<IObserver<DataSet>>();
		editor.DataSetEdited.Subscribe(observer);
		editor.Edit(dataSet, data);
		observer.Received().OnNext(dataSet);
	}

	private static DataSet CreateDataSet(string name = "", string description = "")
	{
		return new ClassifierDataSet
		{
			Name = name,
			Description = description
		};
	}
}