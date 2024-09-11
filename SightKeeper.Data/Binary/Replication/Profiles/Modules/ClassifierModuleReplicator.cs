using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal sealed class ClassifierModuleReplicator : ModuleReplicator
{
	public ClassifierModuleReplicator(ReplicationSession session) : base(session)
	{
	}

	public override Module Replicate(Profile profile, PackableModule packedModule)
	{
		var module = (ClassifierModule)base.Replicate(profile, packedModule);
		// TODO behavior actions
		return module;
	}

	protected override ClassifierModule CreateModule(Profile profile, Weights weights)
	{
		var typedWeights = (PlainWeights<ClassifierTag>)weights;
		return profile.CreateModule(typedWeights);
	}
}