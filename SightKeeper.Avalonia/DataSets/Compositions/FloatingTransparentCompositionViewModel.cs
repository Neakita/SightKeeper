using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.DataSets.Compositions;

internal sealed partial class FloatingTransparentCompositionViewModel : CompositionViewModel
{
	public override string DisplayName => "Floating transparent";

	public TimeSpan SeriesDuration => TimeSpan.FromMilliseconds(SeriesDurationInMilliseconds);
	
	public FloatingTransparentCompositionViewModel()
	{
		_seriesDurationInMilliseconds = 500;
	}

	public FloatingTransparentCompositionViewModel(FloatingTransparentComposition composition) : base(composition)
	{
		_seriesDurationInMilliseconds = (ushort)composition.SeriesDuration.TotalMilliseconds;
	}

	public override FloatingTransparentComposition ToComposition()
	{
		return new FloatingTransparentComposition(MaximumScreenshotsDelay, SeriesDuration, PrimaryOpacity, MinimumOpacity);
	}

	[ObservableProperty] private ushort _seriesDurationInMilliseconds;
	[ObservableProperty] private float _primaryOpacity = 0.5f;
	[ObservableProperty] private float _minimumOpacity = 0.1f;
}