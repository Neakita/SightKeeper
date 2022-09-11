using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Backend.Data;
using SightKeeper.Backend.Data.Members;
using SightKeeper.UI.WPF.ViewModels.Windows;
using SightKeeper.UI.WPF.Views.Popups;

namespace SightKeeper.UI.WPF.ViewModels.Pages;

public class SettingsPageVM : ReactiveObject
{
	public Game[] Games
	{
		get
		{
			using AppDbContext dbContext = new();
			return dbContext.Games.ToArray();
		}
	}
	
	[Reactive] public Game? SelectedGame { get; set; }

	public static IEnumerable<Process> Processes => Process.GetProcesses().Where(process => process.MainWindowHandle != IntPtr.Zero && process.MainWindowTitle != string.Empty && process.Id != SelfProcessId);
	private static readonly int SelfProcessId = Environment.ProcessId;


	private ReactiveCommand<Unit, Unit>? _removeProcessCommand;
	public ReactiveCommand<Unit, Unit> RemoveProcessCommand => _removeProcessCommand ??= ReactiveCommand.Create(() =>
	{
		if (SelectedGame == null) return;
		using AppDbContext dbContext = new();
		dbContext.Games.Remove(SelectedGame);
		dbContext.SaveChanges();
		UpdateGamesList();
	}, this.WhenAnyValue(vm => vm.SelectedGame).Select(selectedGame => selectedGame != null));

	private void UpdateGamesList() => this.RaisePropertyChanged(nameof(Games));

	private ReactiveCommand<Unit, Unit>? _addNewProcessCommand;
	public ReactiveCommand<Unit, Unit> AddNewProcessCommand => _addNewProcessCommand ??= ReactiveCommand.Create(() =>
	{
		AddProcessPopup popup = new(Processes);
		popup.ProcessSelected += OnProcessSelect;
		MainWindowVM.Current.ShowPopup(popup);
	});

	private void OnProcessSelect(AddProcessPopup sender, Process? newProcess)
	{
		sender.ProcessSelected -= OnProcessSelect;
		MainWindowVM.Current.RemovePopup(sender);
		if (newProcess == null) return;
		Game newGame = new(newProcess.MainWindowTitle, newProcess.ProcessName);
		using AppDbContext dbContext = new();
		dbContext.Games.Add(newGame);
		dbContext.SaveChanges();
		UpdateGamesList();
	}
}
