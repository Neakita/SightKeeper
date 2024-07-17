using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class ClassifierModule : Module
{
	public override ClassifierWeights Weights => _weights;
	public TriggerBehaviour Behaviour { get; }

	public void SetWeights(ClassifierWeights weights)
	{
		_weights = weights;
		Behaviour.RemoveInappropriateTags();
	}

	internal ClassifierModule(Profile profile, ClassifierWeights weights) : base(profile)
	{
		_weights = weights;
		Behaviour = new TriggerBehaviour(this);
	}

	private ClassifierWeights _weights;
}