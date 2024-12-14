using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.DataSets.Compositions;

internal abstract partial class CompositionViewModel : ViewModel
{
	public abstract string DisplayName { get; }

	public static CompositionViewModel? Create(ImageComposition? composition) => composition switch
	{
		null => null,
		FixedTransparentImageComposition fixedTransparent => new FixedTransparentCompositionViewModel(fixedTransparent),
		FloatingTransparentImageComposition floatingTransparent => new FloatingTransparentCompositionViewModel(floatingTransparent),
		_ => throw new ArgumentOutOfRangeException(nameof(composition))
	};

	public TimeSpan MaximumScreenshotsDelay => TimeSpan.FromMilliseconds(MaximumScreenshotsDelayInMilliseconds);

	public abstract ImageComposition ToComposition();

	protected CompositionViewModel()
	{
		_maximumScreenshotsDelayInMilliseconds = 50;
	}

	protected CompositionViewModel(ImageComposition imageComposition)
	{
		_maximumScreenshotsDelayInMilliseconds = (ushort)imageComposition.MaximumScreenshotsDelay.TotalMilliseconds;
	}

	[ObservableProperty] private ushort _maximumScreenshotsDelayInMilliseconds;
}