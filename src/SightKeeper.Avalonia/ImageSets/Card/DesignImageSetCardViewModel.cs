using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed partial class DesignImageSetCardViewModel : ViewModel, ImageSetCardDataContext
{
	public static DesignImageSetCardViewModel Instance => new("KF2 FullHD", "kfSample1.jpg");

	public string Name { get; }

	[ObservableProperty] public partial bool IsCapturing { get; private set; }

	public ImageDataContext? PreviewImage => _previewSampleImageFileName == null ? null : new DesignImageDataContext(_previewSampleImageFileName);
	public ICommand EditCommand => new RelayCommand(() => { });
	public ICommand DeleteCommand => new RelayCommand(() => { });
	public ICommand StartCapturingCommand => new RelayCommand(() => IsCapturing = true);
	public ICommand StopCapturingCommand => new RelayCommand(() => IsCapturing = false);

	public DesignImageSetCardViewModel(string name, string? previewSampleImageFileName = null)
	{
		_previewSampleImageFileName = previewSampleImageFileName;
		Name = name;
	}

	private readonly string? _previewSampleImageFileName;
}