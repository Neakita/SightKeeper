using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Images;

public sealed partial class ImagesViewModel : ViewModel, ImagesDataContext, ImageSelection, IDisposable
{
	public IReadOnlyCollection<AnnotationImageDataContext> Images => _images.ToReadOnlyNotifyingList();
	[ObservableProperty] public partial int SelectedImageIndex { get; set; } = -1;
	public Image? SelectedImage => SelectedImageIndex >= 0 ? _set?.Images[SelectedImageIndex] : null;
	public IObservable<Image?> SelectedImageChanged => _selectedImageChanged.AsObservable();

	public ImagesViewModel(ImageSetSelection imageSetSelection, WriteableBitmapImageLoader imageLoader)
	{
		_disposable = imageSetSelection.SelectedImageSetChanged.Subscribe(SetImageSet);
		_imageLoader = imageLoader;
	}

	public void Dispose()
	{
		_disposable.Dispose();
		_selectedImageChanged.Dispose();
	}

	private readonly WriteableBitmapImageLoader _imageLoader;
	private readonly IDisposable _disposable;
	private readonly Subject<Image?> _selectedImageChanged = new();
	private ImageSet? _set;
	private ReadOnlyObservableList<AnnotationImageDataContext> _images = ReadOnlyObservableList<AnnotationImageDataContext>.Empty;

	private void SetImageSet(ImageSet? set)
	{
		_set = set;
		_images.Dispose();
		_images = ReadOnlyObservableList<AnnotationImageDataContext>.Empty;
		if (_set == null)
			return;
		var images = (ReadOnlyObservableList<Image>)_set.Images;
		_images = images.Transform(image => new AnnotationImageViewModel(_imageLoader, image)).ToObservableList();
		OnPropertyChanged(nameof(Images));
	}

	partial void OnSelectedImageIndexChanged(int value)
	{
		var image = value == -1 ? null : _set?.Images[value];
		_selectedImageChanged.OnNext(image);
	}
}