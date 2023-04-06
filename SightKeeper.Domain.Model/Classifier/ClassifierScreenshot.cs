using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Classifier;

public sealed class ClassifierScreenshot : Screenshot
{
	public ClassifierScreenshot(Image image) : base(image)
	{
	}

	public ClassifierScreenshot(int id) : base(id)
	{
	}
}
