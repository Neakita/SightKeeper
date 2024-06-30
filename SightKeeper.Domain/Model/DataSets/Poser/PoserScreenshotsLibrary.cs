namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserScreenshotsLibrary : ScreenshotsLibrary<PoserScreenshot>
{
	public override PoserDataSet DataSet { get; }

	internal PoserScreenshotsLibrary(PoserDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected internal override PoserScreenshot CreateScreenshot()
	{
		PoserScreenshot screenshot = new(this);
		AddScreenshot(screenshot);
		return screenshot;
	}
}