using System.Collections.Immutable;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.DataSets.Compositions;

internal sealed partial class FixedTransparentCompositionViewModel : CompositionViewModel
{
	public override string DisplayName => "Fixed transparent";

	public byte ScreenshotsCount
	{
		get => (byte)Opacities.Count;
		set
		{
			var delta = value - Opacities.Count;
			OnPropertyChanging();
			if (delta > 0)
				Opacities = Opacities.AddRange(Enumerable.Repeat(0.1m, delta));
			else if (delta < 0)
				Opacities = Opacities.RemoveRange(Opacities.Count + delta, -delta);
			OnPropertyChanged();
		}
	}

	public FixedTransparentCompositionViewModel()
	{
		_opacities = [0.1m, 0.2m, 0.7m];
	}

	public FixedTransparentCompositionViewModel(FixedTransparentComposition composition) : base(composition)
	{
		_opacities = composition.Opacities.Select(opacity => (decimal)opacity).ToImmutableList();
	}

	public override FixedTransparentComposition ToComposition() =>
		new(MaximumScreenshotsDelay, 
			Opacities.Select(opacity => (float)opacity).ToImmutableArray());

	[ObservableProperty] private ImmutableList<decimal> _opacities;
}