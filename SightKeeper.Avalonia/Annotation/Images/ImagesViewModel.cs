using System;
using System.Linq;
using System.Reactive;
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

public sealed partial class ImagesViewModel : ViewModel, ImageSelection, AnnotationImagesComponent
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

	public ReadOnlyObservableList<ImageViewModel> Images { get; }
	[ObservableProperty] public partial int SelectedImageIndex { get; set; } = -1;
	public Image? SelectedImage => SelectedImageIndex >= 0 ? Images[SelectedImageIndex].Value : null;
	public ImageViewModel? SelectedImageViewModel => SelectedImage != null ? Images[SelectedImageIndex] : null;
	public IObservable<Unit> SelectedImageChanged => _selectedImageChanged.AsObservable();

	public ImagesViewModel(
		ObservableImageDataAccess observableDataAccess)
	{
		_images = new ObservableList<Image>();
		Images = _images
			.Transform(image => new ImageViewModel(image))
			.ToObservableList();
		observableDataAccess.Added
			.Where(image => image.Set == Set)
			.Subscribe(_images.Add)
			.DisposeWith(_disposable);
		observableDataAccess.Removed
			.Where(image => image.Set == Set)
			.Select(_images.Remove)
			.Subscribe(isRemoved => Guard.IsTrue(isRemoved))
			.DisposeWith(_disposable);
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly ObservableList<Image> _images;
	private readonly Subject<Unit> _selectedImageChanged = new();

	partial void OnSelectedImageIndexChanged(int value)
	{
		_selectedImageChanged.OnNext(Unit.Default);
	}
}