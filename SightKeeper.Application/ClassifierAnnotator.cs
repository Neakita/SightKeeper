using Serilog;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application;

public abstract class ClassifierAnnotator
{
	public virtual void SetTag(Screenshot<ClassifierAsset> screenshot, ClassifierTag? tag)
	{
		if (screenshot.Asset?.Tag == tag)
		{
			_logger.Warning("Unnecessary {methodName} as {memberName} is already equal to {value}",
				nameof(SetTag), nameof(screenshot.Asset.Tag), tag);
			return;
		}
		if (tag != null)
		{
			var asset = screenshot.Asset ?? screenshot.MakeAsset();
			asset.Tag = tag;
			return;
		}
		screenshot.DeleteAsset();
	}

	protected ClassifierAnnotator(ILogger logger)
	{
		_logger = logger;
	}

	private readonly ILogger _logger;
}