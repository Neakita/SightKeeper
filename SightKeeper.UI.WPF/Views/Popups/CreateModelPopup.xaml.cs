using SightKeeper.Backend.Data.Members.Abstract;
using SightKeeper.UI.WPF.ViewModels.Popups;

namespace SightKeeper.UI.WPF.Views.Popups;

public partial class CreateModelPopup
{
	private readonly CreateModelPopupVM _viewModel;
	
	public delegate void ModelHandler(CreateModelPopup sender, Model? model);
	public event ModelHandler? Done
	{
		add => _viewModel.Done += value;
		remove => _viewModel.Done -= value;
	}
	
	public CreateModelPopup()
	{
		InitializeComponent();
		_viewModel = (CreateModelPopupVM) DataContext;
		_viewModel.Initialize(this);
	}
}