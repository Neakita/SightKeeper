using System;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface AnnotationDrawerComponent : DrawerDataContext
{
	AssetsOwner<ItemsOwner>? AssetsLibrary { set; }
	new Screenshot? Screenshot { get; set; }
	Screenshot? DrawerDataContext.Screenshot => Screenshot;
	public Tag? Tag { set; }
	IObservable<BoundedItemDataContext?> SelectedItemChanged { get; }
}