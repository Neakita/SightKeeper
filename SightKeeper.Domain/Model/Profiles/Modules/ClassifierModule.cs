using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class ClassifierModule : Module
{
	public override Weights<ClassifierTag> Weights => _weights;
	public TriggerBehavior Behavior { get; }

	public void SetWeights(Weights<ClassifierTag> weights)
	{
		_weights = weights;
		Behavior.RemoveInappropriateTags();
	}

	internal ClassifierModule(Profile profile, Weights<ClassifierTag> weights) : base(profile)
	{
		_weights = weights;
		Behavior = new TriggerBehavior(this);
	}

	private Weights<ClassifierTag> _weights;
}