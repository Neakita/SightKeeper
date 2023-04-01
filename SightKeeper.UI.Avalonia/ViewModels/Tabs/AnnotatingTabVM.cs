using SightKeeper.Domain.Services;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class AnnotatingTabVM : ViewModel
{
	public Repository<ModelVM> ModelsRepository { get; }

	public AnnotatingTabVM(Repository<ModelVM> modelsRepository)
	{
		ModelsRepository = modelsRepository;
	}
}