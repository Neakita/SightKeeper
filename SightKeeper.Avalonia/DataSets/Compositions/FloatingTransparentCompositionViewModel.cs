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

	public FloatingTransparentCompositionViewModel(FloatingTransparentImageComposition imageComposition) : base(imageComposition)
	{
		_seriesDurationInMilliseconds = (ushort)imageComposition.SeriesDuration.TotalMilliseconds;
	}

	public override FloatingTransparentImageComposition ToComposition()
	{
		return new FloatingTransparentImageComposition(MaximumScreenshotsDelay, SeriesDuration, PrimaryOpacity, MinimumOpacity);
	}

	[ObservableProperty] private ushort _seriesDurationInMilliseconds;
	[ObservableProperty] private float _primaryOpacity = 0.5f;
	[ObservableProperty] private float _minimumOpacity = 0.1f;
}