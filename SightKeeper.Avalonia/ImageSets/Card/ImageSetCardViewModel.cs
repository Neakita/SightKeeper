using System;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

public sealed partial class ImageSetCardViewModel : ViewModel, ImageSetCardDataContext, IDisposable
{
	public ImageSet ImageSet { get; }
	public string Name => ImageSet.Name;

	public bool IsCapturing => _capturer.Set == ImageSet;

	public ImageDataContext? PreviewImage
	{
		get
		{
			var image = ImageSet.Images.RandomOrDefault();
			return image == null ? null : new ImageViewModel(_imageLoader, image);
		}
	}
	public ICommand EditCommand { get; }
	public ICommand DeleteCommand { get; }

	ICommand ImageSetCardDataContext.StartCapturingCommand => StartCapturingCommand;
	ICommand ImageSetCardDataContext.StopCapturingCommand => StopCapturingCommand;

	public ImageSetCardViewModel(ImageSet value, ICommand editCommand, ICommand deleteCommand, ImageLoader imageLoader, ImageCapturer capturer)
	{
		_imageLoader = imageLoader;
		_capturer = capturer;
		ImageSet = value;
		EditCommand = editCommand;
		DeleteCommand = deleteCommand;
		_disposable = capturer.SetChanged.Subscribe(OnCapturerSetChanged);
		if (value is INotifyPropertyChanged notifyingValue)
			notifyingValue.PropertyChanged += OnValuePropertyChanged;
	}

	public void Dispose()
	{
		_disposable.Dispose();
		if (ImageSet is INotifyPropertyChanged notifyingValue)
			notifyingValue.PropertyChanged -= OnValuePropertyChanged;
	}

	private readonly IDisposable _disposable;
	private readonly ImageLoader _imageLoader;
	private readonly ImageCapturer _capturer;

	[RelayCommand]
	private void StartCapturing()
	{
		_capturer.Set = ImageSet;
	}

	[RelayCommand]
	private void StopCapturing()
	{
		_capturer.Set = null;
	}

	private void OnCapturerSetChanged(ImageSet? set)
	{
		OnPropertyChanged(nameof(IsCapturing));
	}

	private void OnValuePropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(ImageSet.Name))
			OnPropertyChanged(args);
	}
}