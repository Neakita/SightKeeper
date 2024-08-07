using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets;

public class DataSetTests
{
	[Fact]
	public void ShouldNotCreateDataSetWithZeroResolution()
	{
		Assert.ThrowsAny<Exception>(() => new DetectorDataSet {Resolution = 0});
	}

	[Fact]
	public void ShouldNotCreateDataSetWithZeroNon32DivisorResolution()
	{
		Assert.ThrowsAny<Exception>(() => new DetectorDataSet {Resolution = 300});
	}
}