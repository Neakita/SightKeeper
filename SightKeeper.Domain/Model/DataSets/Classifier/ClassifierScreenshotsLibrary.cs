namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierScreenshotsLibrary : ScreenshotsLibrary<ClassifierScreenshot>
{
	public override ClassifierDataSet DataSet { get; }

	internal ClassifierScreenshotsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected internal override ClassifierScreenshot CreateScreenshot()
	{
		ClassifierScreenshot screenshot = new(this);
		AddScreenshot(screenshot);
		return screenshot;
	}
}