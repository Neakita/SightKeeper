using System.Collections.Generic;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabVM
{
	public IReadOnlyCollection<Model> Models { get; }
}
