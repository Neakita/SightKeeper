using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.Views.Popups;

namespace SightKeeper.UI.WPF.ViewModels.Popups;

public class AddProcessPopupVM : ReactiveObject
{
	public event AddProcessPopup.ProcessHandler? ProcessSelected;
	
	[Reactive] public IEnumerable<Process>? Processes { get; private set; }
	[Reactive] public Process? SelectedProcess { get; set; }
	
	public ReactiveCommand<Unit, Unit> ApplyCommand => _addProcessCommand ??= ReactiveCommand.Create(() =>
	{
		if (_parentPopup == null) throw new InvalidOperationException();
		ProcessSelected?.Invoke(_parentPopup, SelectedProcess);
	}, this.WhenAnyValue(vm => vm.SelectedProcess).Select(selectedProcess => selectedProcess != null));
	
	public ReactiveCommand<Unit, Unit> CancelCommand => _cancelProcessCommand ??= ReactiveCommand.Create(() =>
	{
		if (_parentPopup == null) throw new InvalidOperationException();
		ProcessSelected?.Invoke(_parentPopup, null);
	});


	public void Initialize(AddProcessPopup parentPopup, IEnumerable<Process> processes)
	{
		_parentPopup = parentPopup;
		Processes = processes;
	}
	
	private ReactiveCommand<Unit, Unit>? _addProcessCommand;
	private ReactiveCommand<Unit, Unit>? _cancelProcessCommand;
	private AddProcessPopup? _parentPopup;
}
