using System.Collections.Generic;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

internal sealed class DetectorDrawerViewModel : DrawerViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public override IReadOnlyCollection<DetectorItemViewModel> Items => _items;
	public override DetectorTag? Tag => _tag;

	public DetectorDrawerViewModel(DetectorAnnotator annotator)
	{
		_annotator = annotator;
	}

	internal ScreenshotViewModel<DetectorAssetViewModel, DetectorAsset>? Screenshot
	{
		get;
		set
		{
			field = value;
			_items.Clear();
			var asset = Screenshot?.Value.Asset;
			if (asset != null)
				_items.AddRange(asset.Items.Select(item => new DetectorItemViewModel(item, _annotator)));
		}
	}

	internal void SetTag(DetectorTag? tag)
	{
		OnPropertyChanging(nameof(Tag));
		_tag = tag;
		OnPropertyChanged(nameof(Tag));
	}

	protected override void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(Screenshot);
		Guard.IsNotNull(Tag);
		var item = _annotator.CreateItem(Screenshot.Value, Tag, bounding);
		DetectorItemViewModel itemViewModel = new(item, _annotator);
		_items.Add(itemViewModel);
	}

	private readonly AvaloniaList<DetectorItemViewModel> _items = new();
	private readonly DetectorAnnotator _annotator;
	private DetectorTag? _tag;
}