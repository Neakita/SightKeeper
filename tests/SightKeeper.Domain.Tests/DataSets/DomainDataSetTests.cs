using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class DomainDataSetTests
{
	[Fact]
	public void ShouldGetNameFromInner()
	{
		var innerSet = Substitute.For<DataSet<Tag, Asset>>();
		const string name = "the name";
		innerSet.Name.Returns(name);
		var domainSet = new DomainDataSet<Tag, Asset>(innerSet, 1);
		domainSet.Name.Returns(name);
	}

	[Fact]
	public void ShouldSetNameToInner()
	{
		var innerSet = Substitute.For<DataSet<Tag, Asset>>();
		const string name = "the name";
		var domainSet = new DomainDataSet<Tag, Asset>(innerSet, 1);
		domainSet.Name = name;
		innerSet.Received().Name = name;
	}

	[Fact]
	public void ShouldGetDescriptionFromInner()
	{
		var innerSet = Substitute.For<DataSet<Tag, Asset>>();
		const string description = "the description";
		innerSet.Description.Returns(description);
		var domainSet = new DomainDataSet<Tag, Asset>(innerSet, 1);
		domainSet.Description.Returns(description);
	}

	[Fact]
	public void ShouldSetDescriptionToInner()
	{
		var innerSet = Substitute.For<DataSet<Tag, Asset>>();
		const string description = "the description";
		var domainSet = new DomainDataSet<Tag, Asset>(innerSet, 1);
		domainSet.Description = description;
		innerSet.Received().Description = description;
	}

	[Fact]
	public void ShouldHaveDomainTagsLibrary()
	{
		var innerSet = Substitute.For<DataSet<Tag, Asset>>();
		var domainSet = new DomainDataSet<Tag, Asset>(innerSet, 1);
		domainSet.TagsLibrary.Should().BeOfType<DomainTagsLibrary<Tag>>();
	}

	[Fact]
	public void ShouldGetAssetsLibraryFromInner()
	{
		var innerSet = Substitute.For<DataSet<Tag, Asset>>();
		var domainSet = new DomainDataSet<Tag, Asset>(innerSet, 1);
		domainSet.AssetsLibrary.Should().Be(innerSet.AssetsLibrary);
	}

	[Fact]
	public void ShouldHaveDomainWeightsLibrary()
	{
		var innerSet = Substitute.For<DataSet<Tag, Asset>>();
		var domainSet = new DomainDataSet<Tag, Asset>(innerSet, 1);
		domainSet.WeightsLibrary.Should().BeOfType<DomainWeightsLibrary>();
	}
}