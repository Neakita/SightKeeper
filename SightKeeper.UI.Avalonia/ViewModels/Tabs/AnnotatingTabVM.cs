using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class AnnotatingTabVM : ViewModel
{
	public Repository<Model> ModelsRepository { get; }

	public AnnotatingTabVM(Repository<Model> modelsRepository)
	{
		ModelsRepository = modelsRepository;
	}
}