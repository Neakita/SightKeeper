using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed class DesignImageSetCardDataContext : ImageSetCardDataContext
{
	public static DesignImageSetCardDataContext Instance => new("KF2 FullHD", "kfSample1.jpg");

	public string Name { get; }
	public ImageDataContext? PreviewImage => _previewSampleImageFileName == null ? null : new DesignImageDataContext(_previewSampleImageFileName);
	public ICommand EditCommand => new RelayCommand(() => { });
	public ICommand DeleteCommand => new RelayCommand(() => { });

	public DesignImageSetCardDataContext(string name, string? previewSampleImageFileName = null)
	{
		_previewSampleImageFileName = previewSampleImageFileName;
		Name = name;
	}

	private readonly string? _previewSampleImageFileName;
}