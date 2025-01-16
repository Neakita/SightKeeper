using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class Poser3DDrawerViewModel : DrawerViewModel
{
	public override IReadOnlyCollection<Poser3DItemViewModel> Items => _items;
	public override PoserTag? Tag => _tag;

	public AssetsLibrary<Poser3DAsset>? AssetsLibrary
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

	public Poser3DDrawerViewModel(Poser3DAnnotator annotator)
	{
		_annotator = annotator;
	}

	public void SetTag(PoserTag tag)
	{
		_tag = tag;
	}

	protected override void CreateItem(Bounding bounding)
	{
		Guard.IsNotNull(AssetsLibrary);
		Guard.IsNotNull(Screenshot);
		Guard.IsNotNull(Tag);
		var item = _annotator.CreateItem(AssetsLibrary, Screenshot, Tag, bounding);
		Poser3DItemViewModel itemViewModel = new(item);
		_items.Add(itemViewModel);
	}

	private readonly AvaloniaList<Poser3DItemViewModel> _items = new();
	private readonly Poser3DAnnotator _annotator;
	private PoserTag? _tag;

	private void UpdateItems()
	{
		_items.Clear();
		if (AssetsLibrary == null ||
		    Screenshot == null ||
		    !AssetsLibrary.Assets.TryGetValue(Screenshot, out var asset))
			return;
		_items.AddRange(asset.Items.Select(CreateItemViewModel));
	}

	private static Poser3DItemViewModel CreateItemViewModel(Poser3DItem item)
	{
		return new Poser3DItemViewModel(item);
	}
}