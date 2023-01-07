using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public interface IModelsListInfo<TModelVM> where TModelVM : ModelVM
{
	string Label { get; }
}