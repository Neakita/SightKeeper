using System;
using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class Poser2DDrawerViewModel : DrawerViewModel
{
	public override IReadOnlyCollection<DrawerItemViewModel> Items => throw new NotImplementedException();
	public override Tag Tag => throw new NotImplementedException();

	protected override void CreateItem(Bounding bounding)
	{
		throw new NotImplementedException();
	}
}