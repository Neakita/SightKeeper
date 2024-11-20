using System.Collections.Generic;
using Avalonia.Collections;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

internal sealed class DetectorDrawerViewModel : DrawerViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public override ScreenshotViewModel<DetectorAssetViewModel, DetectorAsset>? Screenshot
	{
		get;
		set
		{
			if (field == value)
				return;
			field = value;
			OnPropertyChanged();
		}
	}

	public override IReadOnlyCollection<DetectorItemViewModel> Items => _items;

	protected override void CreateItem(Bounding bounding)
	{
		
	}

	private readonly AvaloniaList<DetectorItemViewModel> _items = new();
}