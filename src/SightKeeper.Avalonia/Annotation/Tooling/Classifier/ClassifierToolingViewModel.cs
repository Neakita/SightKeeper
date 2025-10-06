using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling.Classifier;

internal sealed partial class ClassifierToolingViewModel : ViewModel, ClassifierToolingDataContext, IDisposable
{
	public IEnumerable<TagDataContext> Tags => _dataSet.TagsLibrary.Tags.Select(tag => new TagViewModel(tag));

	public TagDataContext? SelectedTag
	{
		get => Asset == null ? null : new TagViewModel(Asset.Tag);
		set
		{
			Guard.IsNotNull(AssetsLibrary);
			Guard.IsNotNull(Image);
			if (value == null)
			{
				AssetsLibrary.DeleteAsset(Image);
				return;
			}
			var asset = AssetsLibrary.GetOrMakeAsset(Image);
			asset.Tag = ((TagViewModel)value).Tag;
		}
	}

	[ObservableProperty, NotifyPropertyChangedFor(nameof(SelectedTag), nameof(IsEnabled))]
	public partial ManagedImage? Image { get; set; }

	public bool IsEnabled => Image != null;

	public ClassifierToolingViewModel(DataSet<Tag, ClassifierAsset> dataSet, ImageSelection imageSelection)
	{
		_dataSet = dataSet;
		Image = imageSelection.SelectedImage;
		imageSelection.SelectedImageChanged
			.Subscribe(_ => Image = imageSelection.SelectedImage)
			.DisposeWith(_constructionDisposable);
		imageSelection.SelectedImageChanged
			.Select(_ => GetAssetsObservable())
			.Switch()
			.Subscribe(_ => OnPropertyChanged(nameof(SelectedTag)))
			.DisposeWith(_constructionDisposable);
	}

	public void Dispose()
	{
		_constructionDisposable.Dispose();
	}

	private readonly DataSet<Tag, ClassifierAsset> _dataSet;
	private readonly CompositeDisposable _constructionDisposable = new();
	private AssetsOwner<ClassifierAsset> AssetsLibrary => _dataSet.AssetsLibrary;
	private ClassifierAsset? Asset => Image == null ? null : AssetsLibrary.GetOptionalAsset(Image);

	private IObservable<Unit> GetAssetsObservable()
	{
		if (Image?.Assets is IObservable<object> assetsObservable)
			return assetsObservable.Select(_ => Unit.Default);
		return Observable.Empty<Unit>();
	}
}