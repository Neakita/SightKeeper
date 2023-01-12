using System.Threading;
using System.Threading.Tasks;
using SightKeeper.Common;
using SightKeeper.UI.WPF.Dialogs;
using SightKeeper.UI.WPF.ViewModels.Dialogs;

namespace SightKeeper.UI.WPF.Views.Dialogs;

public partial class ModelCreator : IDialog<bool>
{
	public ModelCreator(IModelCreatorVM modelCreatorVM)
	{
		InitializeComponent();
		DataContext = modelCreatorVM;
	}

	public string Header => "Create model";
	public bool GetResult() => _resultPender.GetResult();

	public Task<bool> GetResultAsync(CancellationToken cancellationToken = default) =>
		_resultPender.GetResultAsync(cancellationToken);


	private readonly ResultPender<bool> _resultPender = new();
}