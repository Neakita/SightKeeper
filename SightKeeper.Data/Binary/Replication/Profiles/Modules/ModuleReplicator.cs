using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal abstract class ModuleReplicator
{
	public ModuleReplicator(ReplicationSession session)
	{
		Session = session;
	}

	public virtual Module Replicate(Profile profile, PackableModule packedModule)
	{
		Guard.IsNotNull(Session.Weights);
		var weights = Session.Weights[packedModule.WeightsId];
		var module = CreateModule(profile, weights);
		module.PassiveScalingOptions = ConvertPassiveScalingOptions(packedModule.PassiveScalingOptions);
		module.PassiveWalkingOptions = ConvertPassiveWalkingOptions(packedModule.PassiveWalkingOptions);
		return module;
	}

	protected ReplicationSession Session { get; }

	protected abstract Module CreateModule(Profile profile, Weights weights);

	private static PassiveScalingOptions? ConvertPassiveScalingOptions(PackablePassiveScalingOptions? options) => options switch
	{
		null => null,
		PackableConstantScalingOptions constantScalingOptions => new ConstantScalingOptions(constantScalingOptions.Factor),
		PackableIterativeScalingOptions iterativeScalingOptions => new IterativeScalingOptions(iterativeScalingOptions.Initial, iterativeScalingOptions.StepSize, iterativeScalingOptions.StepsCount),
		_ => throw new ArgumentOutOfRangeException(nameof(options))
	};

	private static PassiveWalkingOptions? ConvertPassiveWalkingOptions(PackablePassiveWalkingOptions? options) => options switch
	{
		null => null,
		PackableIterativeWalkingOptions iterativeWalkingOptions => new IterativeWalkingOptions
		{
			OffsetStep = iterativeWalkingOptions.OffsetStep,
			StepsCount = iterativeWalkingOptions.StepsCount
		},
		_ => throw new ArgumentOutOfRangeException(nameof(options))
	};
}