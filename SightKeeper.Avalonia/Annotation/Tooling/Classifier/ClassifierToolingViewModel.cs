using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling.Classifier;

public sealed partial class ClassifierToolingViewModel : ViewModel, ClassifierToolingDataContext, IDisposable
{
	[ObservableProperty, NotifyPropertyChangedFor(nameof(Tags))]
	public partial ClassifierDataSet? DataSet { get; set; }

	public IEnumerable<TagDataContext> Tags
	{
		get
		{
			if (DataSet == null)
				return Enumerable.Empty<TagDataContext>();
			return DataSet.TagsLibrary.Tags.Select(tag => new TagViewModel(tag));
		}
	}

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
	public partial Image? Image { get; set; }

	public bool IsEnabled => Image != null;

	public ClassifierToolingViewModel(ImageSelection imageSelection)
	{
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

	private IObservable<Unit> GetAssetsObservable()
	{
		if (Image?.Assets is IObservable<object> assetsObservable)
			return assetsObservable.Select(_ => Unit.Default);
		return Observable.Empty<Unit>();
	}

	public void Dispose()
	{
		_constructionDisposable.Dispose();
	}

	private AssetsOwner<ClassifierAsset>? AssetsLibrary => DataSet?.AssetsLibrary;
	private readonly CompositeDisposable _constructionDisposable = new();

	private ClassifierAsset? Asset => Image == null ? null : AssetsLibrary?.GetOptionalAsset(Image);
}