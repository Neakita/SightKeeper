using NSubstitute;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests.Services;

public sealed class PeriodicDataSaverTests
{
	[Fact]
	public void ShouldCallSaver()
	{
		var saver = Substitute.For<DataSaver>();
		PeriodicDataSaver periodicSaver = new()
		{
			DataSaver = saver,
			Period = TimeSpan.FromMilliseconds(1)
		};
		periodicSaver.Start();
		periodicSaver.SetDataChanged();
		Thread.Sleep(10);
		saver.Received().Save();
	}

	[Fact]
	public void ShouldNotCallSaverWhenDataNotChanged()
	{
		var saver = Substitute.For<DataSaver>();
		PeriodicDataSaver periodicSaver = new()
		{
			DataSaver = saver,
			Period = TimeSpan.FromMilliseconds(1)
		};
		periodicSaver.Start();
		Thread.Sleep(10);
		saver.DidNotReceive().Save();
	}

	[Fact]
	public void ShouldNotStartTwice()
	{
		PeriodicDataSaver periodicSaver = new()
		{
			DataSaver = Substitute.For<DataSaver>()
		};
		periodicSaver.Start();
		Assert.Throws<ArgumentException>(() => periodicSaver.Start());
	}

	[Fact]
	public void ShouldNotStartAfterDisposing()
	{
		PeriodicDataSaver periodicSaver = new()
		{
			DataSaver = Substitute.For<DataSaver>()
		};
		periodicSaver.Dispose();
		Assert.Throws<ObjectDisposedException>(() => periodicSaver.Start());
	}
}