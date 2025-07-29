using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Classifier;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class NotifyingClassifierDataSetTests
{
	[Fact]
	public void ShouldNotifyNameChange()
	{
		var inner = Substitute.For<StorableClassifierDataSet>();
		var set = new NotifyingClassifierDataSet(inner);
		using var monitor = set.Monitor();
		set.Name = "new name";
		monitor.Should().RaisePropertyChangeFor(dataSet => dataSet.Name);
	}

	[Fact]
	public void ShouldNotNotifyNameChangeWhenNewValueEqualsToOldValue()
	{
		var inner = Substitute.For<StorableClassifierDataSet>();
		const string name = "the name";
		inner.Name.Returns(name);
		var set = new NotifyingClassifierDataSet(inner);
		using var monitor = set.Monitor();
		set.Name = name;
		monitor.Should().NotRaisePropertyChangeFor(dataSet => dataSet.Name);
	}

	[Fact]
	public void ShouldSetNameInInner()
	{
		var inner = Substitute.For<StorableClassifierDataSet>();
		var set = new NotifyingClassifierDataSet(inner);
		const string name = "new name";
		set.Name = name;
		inner.Received().Name = name;
	}

	[Fact]
	public void ShouldNotifyDescriptionChange()
	{
		var inner = Substitute.For<StorableClassifierDataSet>();
		var set = new NotifyingClassifierDataSet(inner);
		using var monitor = set.Monitor();
		set.Description = "new description";
		monitor.Should().RaisePropertyChangeFor(dataSet => dataSet.Description);
	}

	[Fact]
	public void ShouldNotNotifyDescriptionChangeWhenNewValueEqualsToOldValue()
	{
		var inner = Substitute.For<StorableClassifierDataSet>();
		const string description = "the description";
		inner.Description.Returns(description);
		var set = new NotifyingClassifierDataSet(inner);
		using var monitor = set.Monitor();
		set.Description = description;
		monitor.Should().NotRaisePropertyChangeFor(dataSet => dataSet.Description);
	}

	[Fact]
	public void ShouldSetDescriptionInInner()
	{
		var inner = Substitute.For<StorableClassifierDataSet>();
		var set = new NotifyingClassifierDataSet(inner);
		const string description = "new description";
		set.Description = description;
		inner.Received().Description = description;
	}
}