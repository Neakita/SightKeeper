using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using SightKeeper.UI.WPF.ViewModels.Popups;

namespace SightKeeper.UI.WPF.Views.Popups;

public partial class AddProcessPopup : UserControl
{
	public delegate void ProcessHandler(AddProcessPopup sender, Process? process);
	public event ProcessHandler ProcessSelected
	{
		add => _viewModel.ProcessSelected += value;
		remove => _viewModel.ProcessSelected -= value;
	}
	
	public AddProcessPopup(IEnumerable<Process> processes)
	{
		InitializeComponent();
		_viewModel = (AddProcessPopupVM) DataContext;
		_viewModel.Initialize(this, processes);
	}
	
	private readonly AddProcessPopupVM _viewModel;
}