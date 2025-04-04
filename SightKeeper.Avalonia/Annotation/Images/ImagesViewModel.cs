using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.ImageSets;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Images;

public sealed partial class ImagesViewModel : ViewModel, ImagesDataContext, ImageSelection, IDisposable
{
	public ImageSet? Set
	{
		get;
		set
		{
			if (!SetProperty(ref field, value))
				return;
			_images.ReplaceAll(value?.Images ?? Enumerable.Empty<Image>());
		}
	}

	public IReadOnlyCollection<AnnotationImageDataContext> Images { get; }
	[ObservableProperty] public partial int SelectedImageIndex { get; set; } = -1;
	public Image? SelectedImage => SelectedImageIndex >= 0 ? _images[SelectedImageIndex] : null;
	public IObservable<Image> SelectedImageChanged => _selectedImageChanged.AsObservable();

	public ImagesViewModel(ObservableImageDataAccess observableDataAccess, ImageLoader imageLoader)
	{
		_images = new ObservableList<Image>();
		observableDataAccess.Added
			.Where(image => image.Set == Set)
			.Subscribe(_images.Add)
			.DisposeWith(_disposable);
		observableDataAccess.Removed
			.Where(image => image.Set == Set)
			.Select(_images.Remove)
			.Subscribe(isRemoved => Guard.IsTrue(isRemoved))
			.DisposeWith(_disposable);
		var images = _images
			.Transform(image => new AnnotationImageViewModel(imageLoader, image))
			.ToObservableList();
		images.DisposeWith(_disposable);
		Images = images;
	}

	public void Dispose()
	{
		_disposable.Dispose();
		_images.Dispose();
		_selectedImageChanged.Dispose();
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly ObservableList<Image> _images;
	private readonly Subject<Image> _selectedImageChanged = new();

	partial void OnSelectedImageIndexChanged(int value)
	{
		var image = _images[value];
		_selectedImageChanged.OnNext(image);
	}
}