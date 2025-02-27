using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.ImageSets;
using SightKeeper.Domain.Images;

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
			_imagesSource.Edit(source =>
			{
				source.Clear();
				if (value != null)
					source.AddRange(value.Images);
			});
		}
	}

	public IReadOnlyCollection<ImageViewModel> Images { get; }
	[ObservableProperty] public partial int SelectedImageIndex { get; set; } = -1;
	public Image? SelectedImage => SelectedImageIndex >= 0 ? _imagesSource.Items[SelectedImageIndex] : null;
	public ImageViewModel? SelectedImageViewModel => SelectedImage != null ? _imagesCache.Lookup(SelectedImage).Value : null;
	public IObservable<Unit> SelectedImageChanged => _selectedImageChanged.AsObservable();

	public ImagesViewModel(
		ObservableImageDataAccess observableDataAccess,
		IEnumerable<ObservableAnnotator> observableAnnotators)
	{
		_imagesSource = new SourceList<Image>();
		_imagesSource.Connect()
			.Transform(image => new ImageViewModel(image))
			.AddKey(viewModel => viewModel.Value)
			.Bind(out var images)
			.PopulateInto(_imagesCache)
			.DisposeWith(_disposable);
		Images = images;
		observableDataAccess.Added
			.Where(image => image.Set == Set)
			.Subscribe(_imagesSource.Add)
			.DisposeWith(_disposable);
		observableDataAccess.Removed
			.Where(image => image.Set == Set)
			.Select(_imagesSource.Remove)
			.Subscribe(isRemoved => Guard.IsTrue(isRemoved))
			.DisposeWith(_disposable);
		observableAnnotators
			.Select(annotator => annotator.AssetsChanged)
			.Merge()
			.Subscribe(OnImageAssetsChanged)
			.DisposeWith(_disposable);
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly SourceList<Image> _imagesSource;
	private readonly SourceCache<ImageViewModel, Image> _imagesCache = new(viewModel => viewModel.Value);
	private readonly Subject<Unit> _selectedImageChanged = new();

	private void OnImageAssetsChanged(Image image)
	{
		var imageViewModel = _imagesCache.Lookup(image).Value;
		imageViewModel.NotifyAssetsChanged();
	}

	partial void OnSelectedImageIndexChanged(int value)
	{
		_selectedImageChanged.OnNext(Unit.Default);
	}
}