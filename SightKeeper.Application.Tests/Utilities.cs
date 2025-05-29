using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests;

internal static class Utilities
{
	public static ImageSetData CreateImageSetData(string name, string description = "")
	{
		var data = Substitute.For<ImageSetData>();
		data.Name.Returns(name);
		data.Description.Returns(description);
		return data;
	}

	public static ExistingImageSetData CreateExistingImageSetData(ImageSet set, string name, string description = "")
	{
		var data = Substitute.For<ExistingImageSetData>();
		data.Set.Returns(set);
		data.Name.Returns(name);
		data.Description.Returns(description);
		return data;
	}

	public static ReadRepository<T> CreateRepository<T>(IReadOnlyCollection<T> items)
	{
		var repository = Substitute.For<ReadRepository<T>>();
		repository.Items.Returns(items);
		return repository;
	}

	public static void ShouldRoughlyBe<T>(this T[,] value, T[,] expected)
	{
		var lengthX = value.GetLength(0);
		var lengthY = value.GetLength(1);
		lengthX.Should().Be(expected.GetLength(0));
		lengthY.Should().Be(expected.GetLength(1));
		value[0, 0].Should().Be(expected[0, 0]);
		value[lengthX - 1, lengthY - 1].Should().Be(expected[lengthX - 1, lengthY - 1]);
		for (int i = 0; i < 10; i++)
		{
			var x = Random.Shared.Next(lengthX);
			var y = Random.Shared.Next(lengthY);
			value[x, y].Should().Be(expected[x, y]);
		}
	}
}