using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

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

	public Screenshot? Screenshot
	{
		get;
		set
		{
			field = value;
			UpdateItems();
		}
	}

	public DetectorDrawerViewModel(DetectorAnnotator annotator, DetectorDataSet dataSet)
	{
		_annotator = annotator;
		AssetsLibrary = dataSet.AssetsLibrary;
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
		var item = _annotator.CreateItem(AssetsLibrary, Screenshot, Tag, bounding);
		DetectorItemViewModel itemViewModel = new(item, _annotator);
		_items.Add(itemViewModel);
	}

	private readonly AvaloniaList<DetectorItemViewModel> _items = new();
	private readonly DetectorAnnotator _annotator;
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
		return new DetectorItemViewModel(item, _annotator);
	}
}