using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class AnnotatingTabVM : ViewModel
{
	public static AnnotatingTabVM New => Locator.Resolve<AnnotatingTabVM>();
	
	public Repository<Model> ModelsRepository { get; }

	public AnnotatingTabVM(Repository<Model> modelsRepository)
	{
		ModelsRepository = modelsRepository;
	}
}