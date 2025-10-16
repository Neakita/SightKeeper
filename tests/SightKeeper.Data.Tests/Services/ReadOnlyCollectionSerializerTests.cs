using FluentAssertions;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests.Services;

public sealed class ReadOnlyCollectionSerializerTests
{
	[Fact]
	public void ShouldCallItemSerializer()
	{
		var itemSerializer = new SubstituteSerializer<int>();
		var serializer = new ReadOnlyCollectionSerializer<int>(itemSerializer);
		serializer.Serialize([1, 2, 3]);
		itemSerializer.Calls.Should().HaveElementAt(0, 1);
		itemSerializer.Calls.Should().HaveElementAt(1, 2);
		itemSerializer.Calls.Should().HaveElementAt(2, 3);
	}
}