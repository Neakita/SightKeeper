using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.Profiles.Behaviours;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class ClassifierModule : Module
{
	public override ClassifierWeights Weights => _weights;
	public TriggerBehaviour Behaviour { get; }

	public void SetWeights(ClassifierWeights weights, ImmutableDictionary<Tag, Action> action)
	{
		_weights = weights;
		try
		{
			Behaviour.Actions = action;
		}
		catch
		{
			Behaviour.Actions = ImmutableDictionary<Tag, Action>.Empty;
			throw;
		}
	}

	internal ClassifierModule(Profile profile, ClassifierWeights weights) : base(profile)
	{
		_weights = weights;
		Behaviour = new TriggerBehaviour(this);
	}

	private ClassifierWeights _weights;
}