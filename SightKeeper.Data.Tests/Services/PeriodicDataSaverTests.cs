using NSubstitute;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests.Services;

public sealed class PeriodicDataSaverTests
{
	[Fact]
	public void ShouldCallSaver()
	{
		var saver = Substitute.For<DataSaver>();
		using PeriodicDataSaver periodicSaver = new(saver);
		periodicSaver.Period = TimeSpan.FromMilliseconds(1);
		periodicSaver.SetDataChanged();
		Thread.Sleep(10);
		saver.Received().Save();
	}

	[Fact]
	public void ShouldNotCallSaverWhenDataNotChanged()
	{
		var saver = Substitute.For<DataSaver>();
		using PeriodicDataSaver periodicSaver = new(saver);
		periodicSaver.Period = TimeSpan.FromMilliseconds(1);
		Thread.Sleep(10);
		saver.DidNotReceive().Save();
	}
}