using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Avalonia.Annotation.ScreenshottingOptions;

internal sealed partial class ActiveScreenshottingOptionsViewModel : ViewModel
{
	public float MinimumConfidence
	{
		get;
		set
		{
			Guard.IsInRange(value, 0, 1);
			SetProperty(ref field, value);
		}
	}

	[ObservableProperty] private ActiveScalingOptionsViewModel? _scalingOptions;
	[ObservableProperty] private ActiveWalkingOptionsViewModel? _walkingOptions;
}