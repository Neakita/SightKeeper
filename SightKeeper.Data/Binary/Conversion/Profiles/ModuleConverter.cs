using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors;
using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.Profiles.Behaviors;
using SightKeeper.Domain.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal abstract class ModuleConverter
{
	public ModuleConverter(ConversionSession session)
	{
		Session = session;
	}

	public PackableModule Convert(Module module)
	{
		Guard.IsNotNull(Session.WeightsIds);
		var weightsId = Session.WeightsIds[module.Weights];
		var passiveScalingOptions = ConvertPassiveScalingOptions(module.PassiveScalingOptions);
		var passiveWalkingOptions = ConvertPassiveWalkingOptions(module.PassiveWalkingOptions);
		return CreateModule(module, weightsId, passiveScalingOptions, passiveWalkingOptions);
	}

	protected ConversionSession Session { get; }

	protected abstract PackableModule CreateModule(
		Module module,
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions);

	protected static PackableTriggerBehavior ConvertTriggerBehavior(TriggerBehavior behavior)
	{
		// TODO Actions
		return new PackableTriggerBehavior();
	}

	private static PackablePassiveScalingOptions? ConvertPassiveScalingOptions(PassiveScalingOptions? options) => options switch
	{
		null => null,
		ConstantScalingOptions constantScalingOptions => new PackableConstantScalingOptions(constantScalingOptions.Factor),
		IterativeScalingOptions iterativeScalingOptions => new PackableIterativeScalingOptions(iterativeScalingOptions.Initial, iterativeScalingOptions.StepSize, iterativeScalingOptions.StepsCount),
		_ => throw new ArgumentOutOfRangeException(nameof(options))
	};

	private static PackablePassiveWalkingOptions? ConvertPassiveWalkingOptions(PassiveWalkingOptions? options) => options switch
	{
		null => null,
		IterativeWalkingOptions iterativeWalkingOptions => new PackableIterativeWalkingOptions(iterativeWalkingOptions.OffsetStep, iterativeWalkingOptions.StepsCount),
		_ => throw new ArgumentOutOfRangeException(nameof(options))
	};
}