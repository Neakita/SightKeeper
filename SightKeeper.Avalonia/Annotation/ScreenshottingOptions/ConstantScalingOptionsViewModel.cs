using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Avalonia.Annotation.ScreenshottingOptions;

/// <summary>
/// ViewModel for <see cref="ConstantScalingOptions"/>
/// </summary>
internal sealed class ConstantScalingOptionsViewModel : ViewModel
{
	public float Factor
	{
		get => _factor;
		set
		{
			Guard.IsGreaterThan(value, 1);
			_factor = value;
		}
	}

	private float _factor = ConstantScalingOptions.DefaultFactor;
}