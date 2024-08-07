using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class ClassifierModule : Module
{
	public override Weights<ClassifierTag> Weights => _weights;
	public TriggerBehaviour Behaviour { get; }

	public void SetWeights(Weights<ClassifierTag> weights)
	{
		_weights = weights;
		Behaviour.RemoveInappropriateTags();
	}

	internal ClassifierModule(Profile profile, Weights<ClassifierTag> weights) : base(profile)
	{
		_weights = weights;
		Behaviour = new TriggerBehaviour(this);
	}

	private Weights<ClassifierTag> _weights;
}