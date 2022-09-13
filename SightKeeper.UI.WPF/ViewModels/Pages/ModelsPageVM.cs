using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Backend.Data;
using SightKeeper.Backend.Data.Members.Abstract;
using SightKeeper.UI.WPF.ViewModels.Data;
using SightKeeper.UI.WPF.ViewModels.Windows;
using SightKeeper.UI.WPF.Views.Popups;

namespace SightKeeper.UI.WPF.ViewModels.Pages;

public class ModelsPageVM : ReactiveObject
{
	public ModelVM[] Models => DetectorModels.Cast<ModelVM>().ToArray();

	private static DetectorModelVM[] DetectorModels
	{
		get
		{
			using AppDbContext dbContext = new();
			return dbContext.DetectorModels.Select(model => new DetectorModelVM(model)).ToArray();
		}
	}
	
	[Reactive] public ModelVM? SelectedModel { get; set; }

	private ReactiveCommand<Unit, Unit>? _createProfileCommand;
	public ReactiveCommand<Unit, Unit> CreateProfileCommand => _createProfileCommand ??= ReactiveCommand.Create(() =>
	{
		CreateModelPopup popup = new();
		popup.Done += OnCreateModelDone;
		MainWindowVM.Current.ShowPopup(popup);
	});

	private void OnCreateModelDone(CreateModelPopup sender, Model? model)
	{
		sender.Done -= OnCreateModelDone;
		MainWindowVM.Current.RemovePopup(sender);
		if (model == null) return;
		using AppDbContext dbContext = new();
		dbContext.Models.Add(model);
		dbContext.SaveChanges();
		UpdateModels();
	}

	private ReactiveCommand<Unit, Unit>? _deleteProfileCommand;
	public ReactiveCommand<Unit, Unit> DeleteProfileCommand => _deleteProfileCommand ??= ReactiveCommand.Create(() =>
	{
		if (SelectedModel == null) return;
		using AppDbContext dbContext = new();
		dbContext.Models.Remove(SelectedModel.Model);
		dbContext.SaveChanges();
		UpdateModels();
	}, this.WhenAnyValue(vm => vm.SelectedModel).Select(selectedModel => selectedModel != null));

	private void UpdateModels() => this.RaisePropertyChanged(nameof(Models));
}