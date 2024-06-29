using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests;

public class DataSetTests
{
	[Fact]
	public void ShouldNotCreateDataSetWithZeroResolution()
	{
		Assert.ThrowsAny<Exception>(() => new DetectorDataSet("", 0));
	}

	[Fact]
	public void ShouldNotCreateDataSetWithZeroNon32DivisorResolution()
	{
		Assert.ThrowsAny<Exception>(() => new DetectorDataSet("", 300));
	}
}