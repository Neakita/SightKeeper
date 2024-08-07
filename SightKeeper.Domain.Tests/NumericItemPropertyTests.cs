using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests;

public sealed class NumericItemPropertyTests
{
	[Fact]
	public void ShouldGetSameValueAfterNormalizing()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var property = tag.CreateProperty("", 100, 300);
		const double value = 200;
		var normalizedValue = property.GetNormalizedValue(value);
		property.GetRangedValue(normalizedValue).Should().Be(value);
	}
}