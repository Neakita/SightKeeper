using System.Collections.Generic;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal abstract class PoserItemViewModel : DrawerItemViewModel
{
	public abstract override PoserTag Tag { get; }
	public abstract IReadOnlyList<KeyPointViewModel> KeyPoints { get; }
}