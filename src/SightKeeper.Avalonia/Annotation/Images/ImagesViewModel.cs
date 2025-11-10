using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.Annotation.Tooling.ImageSet;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Images;

internal sealed partial class ImagesViewModel : ViewModel, ImagesDataContext, ImageSelection, IDisposable
{
	public IReadOnlyCollection<AnnotationImageDataContext> Images => _images.ToReadOnlyNotifyingList();
	[ObservableProperty] public partial int SelectedImageIndex { get; set; } = -1;
	public ManagedImage? SelectedImage => SelectedImageIndex >= 0 ? _set?.Images[SelectedImageIndex] : null;
	public IObservable<ManagedImage?> SelectedImageChanged => _selectedImageChanged.AsObservable();

	public ImagesViewModel(ImageSetSelection imageSetSelection, Func<ManagedImage, AnnotationImageViewModel> imageViewModelFactory)
	{
		_disposable = imageSetSelection.SelectedImageSetChanged.Subscribe(SetImageSet);
		_imageViewModelFactory = imageViewModelFactory;
	}

	public void Dispose()
	{
		_disposable.Dispose();
		_selectedImageChanged.Dispose();
	}

	private readonly Func<ManagedImage, AnnotationImageViewModel> _imageViewModelFactory;
	private readonly IDisposable _disposable;
	private readonly Subject<ManagedImage?> _selectedImageChanged = new();
	private ImageSet? _set;
	private ReadOnlyObservableList<AnnotationImageDataContext> _images = ReadOnlyObservableList<AnnotationImageDataContext>.Empty;

	private void SetImageSet(ImageSet? set)
	{
		_set = set;
		_images.Dispose();
		_images = ReadOnlyObservableList<AnnotationImageDataContext>.Empty;
		if (_set == null)
			return;
		var images = (ReadOnlyObservableList<ManagedImage>)_set.Images;
		_images = images
			.Transform(_imageViewModelFactory)
			.DisposeMany()
			.ToObservableList();
		OnPropertyChanged(nameof(Images));
	}

	partial void OnSelectedImageIndexChanged(int value)
	{
		var image = value == -1 ? null : _set?.Images[value];
		_selectedImageChanged.OnNext(image);
	}
}