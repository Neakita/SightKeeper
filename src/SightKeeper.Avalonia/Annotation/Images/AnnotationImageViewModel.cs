using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using SightKeeper.Avalonia.Annotation.Tooling.DataSet;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using Vibrance;

namespace SightKeeper.Avalonia.Annotation.Images;

internal sealed class AnnotationImageViewModel : ViewModel, AnnotationImageDataContext, IDisposable
{
	public DateTimeOffset CreationTimestamp => _image.CreationTimestamp;
	public bool IsAsset => _dataSetSelection.SelectedDataSet?.AssetsLibrary.Contains(_image) ?? false;

	public AnnotationImageViewModel(WriteableBitmapImageLoader imageLoader, ManagedImage image, DataSetSelection dataSetSelection)
	{
		_imageLoader = imageLoader;
		_image = image;
		_dataSetSelection = dataSetSelection;
		dataSetSelection.SelectedDataSetChanged
			.Subscribe(_ => OnPropertyChanged(nameof(IsAsset)))
			.DisposeWith(_disposable);
		var observableAssets = (ReadOnlyObservableCollection<Asset>)image.Assets;
		observableAssets
			.Subscribe(_ => OnPropertyChanged(nameof(IsAsset)))
			.DisposeWith(_disposable);
	}

	public async Task<Bitmap?> LoadAsync(int? maximumLargestDimension, CancellationToken cancellationToken)
	{
		return await _imageLoader.LoadImageAsync(_image, maximumLargestDimension, cancellationToken);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly WriteableBitmapImageLoader _imageLoader;
	private readonly ManagedImage _image;
	private readonly DataSetSelection _dataSetSelection;
	private readonly CompositeDisposable _disposable = new();
}