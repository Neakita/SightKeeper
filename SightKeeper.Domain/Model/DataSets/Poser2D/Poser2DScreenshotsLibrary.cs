namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DScreenshotsLibrary : ScreenshotsLibrary<Poser2DScreenshot>
{
	public override Poser2DDataSet DataSet { get; }

	internal Poser2DScreenshotsLibrary(Poser2DDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected internal override Poser2DScreenshot CreateScreenshot()
	{
		Poser2DScreenshot screenshot = new(this);
		AddScreenshot(screenshot);
		return screenshot;
	}
}