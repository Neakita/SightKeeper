using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class ClassifierModule : Module
{
	public override PlainWeights<ClassifierTag> Weights => _weights;
	public override TriggerBehavior Behavior { get; }

	public void SetWeights(PlainWeights<ClassifierTag> weights)
	{
		_weights = weights;
		Behavior.RemoveInappropriateTags();
	}

	internal ClassifierModule(Profile profile, PlainWeights<ClassifierTag> weights) : base(profile)
	{
		_weights = weights;
		Behavior = new TriggerBehavior(this);
	}

	private PlainWeights<ClassifierTag> _weights;
}