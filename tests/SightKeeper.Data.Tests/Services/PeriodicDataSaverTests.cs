using NSubstitute;
using Serilog;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests.Services;

public sealed class PeriodicDataSaverTests
{
	[Fact]
	public void ShouldCallSaver()
	{
		var saver = Substitute.For<DataSaver>();
		using PeriodicDataSaver periodicSaver = new(new Lazy<DataSaver>(saver), Substitute.For<ILogger>());
		periodicSaver.Period = TimeSpan.FromMilliseconds(1);
		periodicSaver.Start();
		periodicSaver.SetDataChanged();
		Thread.Sleep(50);
		saver.Received().Save();
	}

	[Fact]
	public void ShouldNotCallSaverWhenDataNotChanged()
	{
		var saver = Substitute.For<DataSaver>();
		using PeriodicDataSaver periodicSaver = new(new Lazy<DataSaver>(saver), Substitute.For<ILogger>());
		periodicSaver.Period = TimeSpan.FromMilliseconds(1);
		periodicSaver.Start();
		Thread.Sleep(10);
		saver.DidNotReceive().Save();
	}

	[Fact]
	public void ShouldThrowAnExceptionIfSettingDataChangedWhenNotStarted()
	{
		var saver = Substitute.For<DataSaver>();
		using PeriodicDataSaver periodicSaver = new(new Lazy<DataSaver>(saver), Substitute.For<ILogger>());
		Assert.Throws<InvalidOperationException>(() => periodicSaver.SetDataChanged());
	}

	[Fact]
	public void ShouldNotCallSaverIfSettingDataChangedWhenNotStarted()
	{
		var saver = Substitute.For<DataSaver>();
		using PeriodicDataSaver periodicSaver = new(new Lazy<DataSaver>(saver), Substitute.For<ILogger>());
		periodicSaver.Period = TimeSpan.FromMilliseconds(1);
		try
		{
			periodicSaver.SetDataChanged();
		}
		catch
		{
			// ignored
		}
		periodicSaver.Start();
		Thread.Sleep(50);
		saver.DidNotReceive().Save();
	}
}