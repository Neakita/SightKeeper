using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Backend.Data;
using SightKeeper.Backend.Data.Members;
using SightKeeper.Backend.Data.Members.Common;
using SightKeeper.Backend.Data.Members.Detector;
using SightKeeper.UI.WPF.Views.Popups;

namespace SightKeeper.UI.WPF.ViewModels.Popups;

public class CreateModelPopupVM : ReactiveObject
{
	private const string DefaultModelName = "Untitled model";
	
	
	public event CreateModelPopup.ModelHandler? Done;
	
	[Reactive] public string Name { get; set; } = DefaultModelName;

	[Reactive] public ushort Width { get; set; } = 320;
	[Reactive] public ushort Height { get; set; } = 320;

	public string[] ModelTypes { get; } = Enum.GetNames<ModelType>();

	public Game[] Games
	{
		get
		{
			using AppDbContext dbContext = new();
			return dbContext.Games.ToArray();
		}
	}
	[Reactive] public Game? SelectedGame { get; set; }
	[Reactive] public string SelectedModelTypeInString { get; set; } = string.Empty;
	[ObservableAsProperty] public ModelType? SelectedModelType { get; } = null!;
	[ObservableAsProperty] public bool IsValid { get; } = false;


	private ReactiveCommand<Unit, Unit>? _applyCommand;
	public ReactiveCommand<Unit, Unit> ApplyCommand => _applyCommand ??= ReactiveCommand.Create(() =>
	{
		if (string.IsNullOrWhiteSpace(Name) || SelectedModelType == null) return;
		Done?.Invoke(_parentPopup, new DetectorModel(Name, new Resolution(Width, Height)));
	}, this.WhenAnyValue(vm => vm.IsValid));
	
	private ReactiveCommand<Unit, Unit>? _cancelCommand;
	public ReactiveCommand<Unit, Unit> CancelCommand => _cancelCommand ??= ReactiveCommand.Create(() => Done?.Invoke(_parentPopup, null));

	public void Initialize(CreateModelPopup parentPopup)
	{
		_parentPopup = parentPopup;
		this.WhenAnyValue(vm => vm.SelectedModelTypeInString)
			.Select(selectedModelTypeInString => Enum.TryParse(selectedModelTypeInString, out ModelType enumValue)
				? enumValue
				: (ModelType?) null)
			.ToPropertyEx(this, vm => vm.SelectedModelType);

		this.WhenAnyValue(vm => vm.Name, vm => vm.SelectedModelType, vm => vm.Width, vm => vm.Height)
			.Select(tuple =>
				!string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item2 != null &&
				tuple.Item3 % 32 == 0 && tuple.Item3 > 0 && tuple.Item4 % 32 == 0 && tuple.Item4 > 0)
			.ToPropertyEx(this, vm => vm.IsValid);
	}


	private CreateModelPopup _parentPopup = null!;
}
