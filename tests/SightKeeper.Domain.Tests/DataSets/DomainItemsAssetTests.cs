using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class DomainItemsAssetTests
{
	[Fact]
	public void ShouldGetImageFromInner()
	{
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		domainAsset.Image.Should().Be(innerAsset.Image);
	}

	[Fact]
	public void ShouldGetUsageFromInner()
	{
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		domainAsset.Usage.Should().Be(innerAsset.Usage);
	}

	[Fact]
	public void ShouldSetUsageToInner()
	{
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		const AssetUsage usage = AssetUsage.Validation;
		domainAsset.Usage = usage;
		innerAsset.Received().Usage = usage;
	}

	[Fact]
	public void ShouldGetItemsFromInner()
	{
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		domainAsset.Items.As<object>().Should().Be(innerAsset.Items);
	}

	[Fact]
	public void ShouldGetInner()
	{
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		domainAsset.Inner.Should().Be(innerAsset);
	}

	[Fact]
	public void ShouldAllowMakeItem()
	{
		var tag = Substitute.For<Tag>();
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		tagsContainer.Tags.Returns([tag]);
		tag.Owner.Returns(tagsContainer);
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		domainAsset.MakeItem(tag);
		innerAsset.Received().MakeItem(tag);
	}

	[Fact]
	public void ShouldDisallowMakeItem()
	{
		var tag = Substitute.For<Tag>();
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		var exception = Assert.Throws<UnexpectedTagsOwnerException>(() => domainAsset.MakeItem(tag));
		innerAsset.DidNotReceive().MakeItem(tag);
		exception.Causer.Should().Be(tag);
		exception.ExpectedOwner.Should().Be(tagsContainer);
	}

	[Fact]
	public void ShouldDeleteItemByIndexInInner()
	{
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		domainAsset.DeleteItemAt(3);
		innerAsset.Received().DeleteItemAt(3);
	}

	[Fact]
	public void ShouldClearItemsInInner()
	{
		var tagsContainer = Substitute.For<TagsContainer<Tag>>();
		var innerAsset = Substitute.For<ItemsAsset<AssetItem>>();
		var domainAsset = new DomainItemsAsset<AssetItem>(tagsContainer, innerAsset);
		domainAsset.ClearItems();
		innerAsset.Received().ClearItems();
	}
}