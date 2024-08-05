namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DScreenshotsLibrary : ScreenshotsLibrary<Poser3DScreenshot>
{
	public override Poser3DDataSet DataSet { get; }

	internal Poser3DScreenshotsLibrary(Poser3DDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected internal override Poser3DScreenshot CreateScreenshot()
	{
		Poser3DScreenshot screenshot = new(this);
		AddScreenshot(screenshot);
		return screenshot;
	}
}