using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.DataSets.Compositions;

internal abstract partial class CompositionViewModel : ViewModel
{
	public abstract string DisplayName { get; }

	public static CompositionViewModel? Create(Composition? composition) => composition switch
	{
		null => null,
		FixedTransparentComposition fixedTransparent => new FixedTransparentCompositionViewModel(fixedTransparent),
		FloatingTransparentComposition floatingTransparent => new FloatingTransparentCompositionViewModel(floatingTransparent),
		_ => throw new ArgumentOutOfRangeException(nameof(composition))
	};

	public TimeSpan MaximumScreenshotsDelay => TimeSpan.FromMilliseconds(MaximumScreenshotsDelayInMilliseconds);

	public abstract Composition ToComposition();

	protected CompositionViewModel()
	{
		_maximumScreenshotsDelayInMilliseconds = 50;
	}

	protected CompositionViewModel(Composition composition)
	{
		_maximumScreenshotsDelayInMilliseconds = (ushort)composition.MaximumScreenshotsDelay.TotalMilliseconds;
	}

	[ObservableProperty] private ushort _maximumScreenshotsDelayInMilliseconds;
}