using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public sealed class DetectorModelVMImplementation : ReactiveValidationObject, DetectorModelVM
{
	public string Name
	{
		get => Item.Name;
		set
		{
			Item.Name = value;
			this.RaisePropertyChanged();
		}
	}

	public ushort Width
	{
		get => Item.Resolution.Width;
		set
		{
			Item.Resolution.Width = value;
			this.RaisePropertyChanged();
		}
	}

	public ushort Height
	{
		get => Item.Resolution.Height;
		set
		{
			Item.Resolution.Height = value;
			this.RaisePropertyChanged();
		}
	}

	public ModelConfig? Config
	{
		get => Item.Config;
		set
		{
			Item.Config = value;
			this.RaisePropertyChanged();
		}
	}

	public Game? Game
	{
		get => Item.Game;
		set
		{
			Item.Game = value;
			this.RaisePropertyChanged();
		}
	}

	public void UpdateProperties()
	{
		this.RaisePropertyChanged(nameof(Name));
	}

	public DetectorModel Item { get; }
	
	public DetectorModelVMImplementation(DetectorModel item)
	{
		Item = item;
		this.ValidationRule(
			viewModel => viewModel.Width,
			width => width > 0 && width % 32 == 0,
			"Width must be greater than 0 and multiple of 32");
		
		this.ValidationRule(
			viewModel => viewModel.Height,
			height => height > 0 && height % 32 == 0,
			"Height must be greater than 0 and multiple of 32");
	}

	Model ItemVM<Model>.Item => Item;
}