using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

internal sealed class DetectorDrawerViewModel : DrawerViewModel
{
	public override IReadOnlyCollection<DetectorItemViewModel> Items => _items;
	public override Tag? Tag => _tag;

	public DetectorDrawerViewModel(DetectorAnnotator annotator, DetectorDataSet dataSet)
	{
		_annotator = annotator;
		_assetsLibrary = dataSet.AssetsLibrary;
	}

	internal ScreenshotViewModel? Screenshot
	{
		get;
		set
		{
			field = value;
			_items.Clear();
			if (value != null && _assetsLibrary.Assets.TryGetValue(value.Value, out var asset))
				_items.AddRange(asset.Items.Select(item => new DetectorItemViewModel(item, _annotator)));
		}
	}

	internal void SetTag(Tag? tag)
	{
		OnPropertyChanging(nameof(Tag));
		_tag = tag;
		OnPropertyChanged(nameof(Tag));
	}

	protected override void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(Screenshot);
		Guard.IsNotNull(Tag);
		var item = _annotator.CreateItem(_assetsLibrary, Screenshot.Value, Tag, bounding);
		DetectorItemViewModel itemViewModel = new(item, _annotator);
		_items.Add(itemViewModel);
	}

	private readonly AvaloniaList<DetectorItemViewModel> _items = new();
	private readonly DetectorAnnotator _annotator;
	private readonly AssetsLibrary<DetectorAsset> _assetsLibrary;
	private Tag? _tag;
}