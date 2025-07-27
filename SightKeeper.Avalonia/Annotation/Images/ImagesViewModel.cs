using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Images;

public sealed partial class ImagesViewModel : ViewModel, ImagesDataContext, ImageSelection, IDisposable
{
	private readonly ImageLoader _imageLoader;

	public ImageSet? Set
	{
		get;
		set
		{
			if (!SetProperty(ref field, value))
				return;
			Images.Dispose();
			Images = ReadOnlyObservableList<AnnotationImageDataContext>.Empty;
			if (Set == null)
				return;
			var images = (ReadOnlyObservableList<Image>)Set.Images;
			Images = images.Transform(image => new AnnotationImageViewModel(_imageLoader, image)).ToObservableList();
		}
	}

	[ObservableProperty]
	public partial ReadOnlyObservableList<AnnotationImageDataContext> Images { get; private set; } =
		ReadOnlyObservableList<AnnotationImageDataContext>.Empty;

	IReadOnlyCollection<AnnotationImageDataContext> ImagesDataContext.Images => Images;
	[ObservableProperty] public partial int SelectedImageIndex { get; set; } = -1;
	public Image? SelectedImage => SelectedImageIndex >= 0 ? Set?.Images[SelectedImageIndex] : null;
	public IObservable<Image?> SelectedImageChanged => _selectedImageChanged.AsObservable();

	public ImagesViewModel(ImageLoader imageLoader)
	{
		_imageLoader = imageLoader;
	}

	public void Dispose()
	{
		_disposable.Dispose();
		_selectedImageChanged.Dispose();
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly Subject<Image?> _selectedImageChanged = new();

	partial void OnSelectedImageIndexChanged(int value)
	{
		var image = value == -1 ? null : Set?.Images[value];
		_selectedImageChanged.OnNext(image);
	}
}