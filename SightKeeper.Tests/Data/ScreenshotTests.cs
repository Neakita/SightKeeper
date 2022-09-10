using System.Drawing;
using SightKeeper.Backend.Data.Members.Abstract;
using SightKeeper.Backend.Data.Members.Detector;

namespace SightKeeper.Tests.Data;

public class ScreenshotTests
{
	[Fact]
	public void ScreenshotFileSetShouldCreateFile()
	{
		// assign
		Screenshot screenshot = new DetectorScreenshot {Id = Guid.NewGuid(), Width = 320, Height = 320};
		using Bitmap bitmap = new(320, 320);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.DrawRectangle(Pens.Brown, 0, 0, 320, 320);
		screenshot.EnsureDeleted();
		
		// act
		screenshot.File = bitmap;
		
		// assert
		Assert.True(screenshot.IsExists);
		
		// clean-up
		screenshot.EnsureDeleted();
	}
	
	[Fact]
	public void ScreenshotFileSetToNullShouldDeleteFile()
	{
		// assign
		Screenshot screenshot = new DetectorScreenshot {Id = Guid.NewGuid(), Width = 320, Height = 320};
		using Bitmap bitmap = new(320, 320);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.DrawRectangle(Pens.Brown, 0, 0, 320, 320);
		screenshot.File = bitmap;
		Assert.True(screenshot.IsExists);
		
		// act
		screenshot.File = null;
		
		// assert
		Assert.False(screenshot.IsExists);
	}
}
