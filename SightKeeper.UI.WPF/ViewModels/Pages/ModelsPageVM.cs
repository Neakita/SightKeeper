using System.Collections.Generic;
using ReactiveUI;
using SightKeeper.UI.WPF.ViewModels.Domain;
using SightKeeper.UI.WPF.ViewModels.Elements;

namespace SightKeeper.UI.WPF.ViewModels.Pages;

public sealed class ModelsPageVM : ReactiveObject
{
	public ModelsPageVM(IModelsList<DetectorModelVM> detectorModelsList, IModelsList<ClassifierModelVM> classifierModelsList)
	{
		ModelsLists = new IModelsList[] {detectorModelsList, classifierModelsList};
	}
	
	public IEnumerable<IModelsList> ModelsLists { get; }
}