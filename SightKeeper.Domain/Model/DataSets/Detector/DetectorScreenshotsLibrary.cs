namespace SightKeeper.Domain.Model.DataSets.Detector;

public class DetectorScreenshotsLibrary : ScreenshotsLibrary<DetectorScreenshot>
{
	public override DetectorDataSet DataSet { get; }

	internal DetectorScreenshotsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected internal override DetectorScreenshot CreateScreenshot()
	{
		DetectorScreenshot screenshot = new(this);
		AddScreenshot(screenshot);
		return screenshot;
	}
}