using FluentAssertions;
using SightKeeper.Backend;
using SightKeeper.Backend.Windows;
using SightKeeper.DAL.Domain.Common;
using Image = SightKeeper.DAL.Image;

namespace SightKeeper.Tests.Backend.Screenshots;

public sealed class ScreenshoterTests
{
	[Fact]
	public void ShouldGetSomeScreenshot()
	{
		IScreenshoter screenshoter = new Screenshoter(new Resolution());

		Image image = screenshoter.MakeScreenshot();
		
		image.Content.Should().NotBeNull();
	}
}
