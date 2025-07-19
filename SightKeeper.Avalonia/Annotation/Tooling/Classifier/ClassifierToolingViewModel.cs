using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
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

	public ClassifierToolingViewModel(ImagesViewModel imagesViewModel)
	{
		_disposable = imagesViewModel.SelectedImageChanged.Subscribe(_ => Image = imagesViewModel.SelectedImage);
		Image = imagesViewModel.SelectedImage;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private AssetsOwner<ClassifierAsset>? AssetsLibrary => DataSet?.AssetsLibrary;
	private readonly IDisposable _disposable;

	private ClassifierAsset? Asset =>
		Image == null ? null : AssetsLibrary?.GetOrMakeAsset(Image);
}