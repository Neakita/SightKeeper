using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

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

	public TimeSpan MaximumDelay => TimeSpan.FromMilliseconds(MaximumDelayInMilliseconds);

	public abstract ImageComposition ToComposition();

	protected CompositionViewModel()
	{
		_maximumDelayInMilliseconds = 50;
	}

	protected CompositionViewModel(ImageComposition imageComposition)
	{
		_maximumDelayInMilliseconds = (ushort)imageComposition.MaximumDelay.TotalMilliseconds;
	}

	[ObservableProperty] private ushort _maximumDelayInMilliseconds;
}