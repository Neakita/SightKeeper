using System.Reactive.Linq;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Misc;

namespace SightKeeper.Application.Tests;

public sealed class ComposeObservableListRepositoryTests
{
	[Fact]
	public void ShouldAddInitialItemsFromReadRepository()
	{
		var observableListRepository = CreateRepository([0, 1, 2]);
		observableListRepository.Items.Should().Contain([0, 1, 2]);
	}

	[Fact]
	public void ShouldAddItemsFromObservableRepository()
	{
		var observableListRepository = CreateRepository([0, 1], [2, 3]);
		observableListRepository.Items.Should().Contain([0, 1, 2, 3]);
	}

	[Fact]
	public void ShouldRemoveItemsFromObservableRepository()
	{
		var observableListRepository = CreateRepository([0, 1, 2, 3], [1, 2]);
		observableListRepository.Items.Should().Contain([0, 3]);
	}

	private static ComposeObservableListRepository<T> CreateRepository<T>(
		IReadOnlyCollection<T> initialItems,
		IEnumerable<T>? addItems = null,
		IEnumerable<T>? removeItems = null)
	{
		var readRepository = Substitute.For<ReadRepository<T>>();
		readRepository.Items.Returns(initialItems);
		var observableRepository = Substitute.For<ObservableRepository<T>>();
		observableRepository.Added.Returns(addItems?.ToObservable() ?? Observable.Empty<T>());
		observableRepository.Removed.Returns(removeItems?.ToObservable() ?? Observable.Empty<T>());
		ComposeObservableListRepository<T> observableListRepository = new(observableRepository, readRepository);
		return observableListRepository;
	}
}