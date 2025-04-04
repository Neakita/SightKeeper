using System;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface AnnotationDrawerComponent
{
	AssetsOwner<ItemsOwner>? AssetsLibrary { set; }
	Image? Image { set; }
	Tag? Tag { set; }
	IObservable<BoundedItemDataContext?> SelectedItemChanged { get; }
	BoundedItemDataContext? SelectedItem { get; }
}