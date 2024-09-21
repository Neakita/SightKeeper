using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Avalonia.Annotation.ScreenshottingOptions;

internal sealed partial class ActiveScreenshottingOptionsViewModel : ViewModel
{
	public float MinimumConfidence
	{
		get => _minimumConfidence;
		set
		{
			Guard.IsInRange(value, 0, 1);
			SetProperty(ref _minimumConfidence, value);
		}
	}

	public ActiveScreenshottingOptionsViewModel(Weights weights)
	{
		_weights = weights;
	}

	private readonly Weights _weights;
	private float _minimumConfidence;
	[ObservableProperty] private ActiveScalingOptionsViewModel? _scalingOptions;
	[ObservableProperty] private ActiveWalkingOptionsViewModel? _walkingOptions;
}