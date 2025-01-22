using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

public sealed class DetectorDrawerViewModel : DrawerViewModel
{
	public override IReadOnlyCollection<DetectorItemViewModel> Items => _items;
	public override Tag? Tag => _tag;

	public AssetsLibrary<DetectorAsset>? AssetsLibrary
	{
		get;
		set
		{
			field = value;
			UpdateItems();
		}
	}

	public DetectorDrawerViewModel(BoundingAnnotator boundingAnnotator, BoundingEditor boundingEditor)
	{
		_boundingAnnotator = boundingAnnotator;
		_boundingEditor = boundingEditor;
	}

	public void SetTag(Tag? tag)
	{
		OnPropertyChanging(nameof(Tag));
		_tag = tag;
		OnPropertyChanged(nameof(Tag));
	}

	protected override void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(AssetsLibrary);
		Guard.IsNotNull(Screenshot);
		Guard.IsNotNull(Tag);
		var item = (DetectorItem)_boundingAnnotator.CreateItem(AssetsLibrary, Screenshot, Tag, bounding);
		DetectorItemViewModel itemViewModel = new(item, _boundingEditor);
		_items.Add(itemViewModel);
	}

	protected override void OnScreenshotChanged()
	{
		UpdateItems();
	}

	private readonly AvaloniaList<DetectorItemViewModel> _items = new();
	private readonly BoundingAnnotator _boundingAnnotator;
	private readonly BoundingEditor _boundingEditor;
	private Tag? _tag;

	private void UpdateItems()
	{
		_items.Clear();
		if (AssetsLibrary == null ||
		    Screenshot == null ||
		    !AssetsLibrary.Assets.TryGetValue(Screenshot, out var asset))
			return;
		_items.AddRange(asset.Items.Select(CreateItemViewModel));
	}

	private DetectorItemViewModel CreateItemViewModel(DetectorItem item)
	{
		return new DetectorItemViewModel(item, _boundingEditor);
	}
}