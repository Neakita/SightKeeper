using ReactiveUI;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public sealed class DetectorModelVM : ReactiveObject, ModelVM<DetectorModel>
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

	Model ModelVM.Model => Model;
	public DetectorModel Model { get; }
	
	public DetectorModelVM(DetectorModel model)
	{
		Model = model;
	}
}