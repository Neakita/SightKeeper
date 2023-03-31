using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public sealed class DetectorModelVM : ReactiveValidationObject, ModelVM<DetectorModel>
{
	public string Name
	{
		get => Model.Name;
		set
		{
			Model.Name = value;
			this.RaisePropertyChanged();
		}
	}

	public ushort Width
	{
		get => Model.Resolution.Width;
		set
		{
			Model.Resolution.Width = value;
			this.RaisePropertyChanged();
		}
	}

	public ushort Height
	{
		get => Model.Resolution.Height;
		set
		{
			Model.Resolution.Height = value;
			this.RaisePropertyChanged();
		}
	}

	public ModelConfig Config
	{
		get => Model.Config;
		set
		{
			Model.Config = value;
			this.RaisePropertyChanged();
		}
	}

	public Game? Game
	{
		get => Model.Game;
		set
		{
			Model.Game = value;
			this.RaisePropertyChanged();
		}
	}

	Model ModelVM.Model => Model;
	public void UpdateProperties()
	{
		this.RaisePropertyChanged(nameof(Name));
	}

	public DetectorModel Model { get; }
	
	public DetectorModelVM(DetectorModel model)
	{
		Model = model;
		this.ValidationRule(
			viewModel => viewModel.Width,
			width => width > 0 && width % 32 == 0,
			"Width must be greater than 0 and multiple of 32");
		
		this.ValidationRule(
			viewModel => viewModel.Height,
			height => height > 0 && height % 32 == 0,
			"Height must be greater than 0 and multiple of 32");
	}
}