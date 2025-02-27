using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class ClassifierAnnotationViewModel : ViewModel, TagSelectionToolingDataContext, IDisposable
{
	[ObservableProperty, NotifyPropertyChangedFor(nameof(Tags))]
	public partial ClassifierDataSet? DataSet { get; set; }

	public IReadOnlyCollection<Tag> Tags => DataSet?.TagsLibrary.Tags ?? ReadOnlyCollection<Tag>.Empty;

	IReadOnlyCollection<Named> TagSelectionToolingDataContext.Tags => Tags;

	public Tag? SelectedTag
	{
		get => Asset?.Tag;
		set
		{
			Guard.IsNotNull(AssetsLibrary);
			Guard.IsNotNull(Image);
			if (value == null)
				_annotator.DeleteAsset(AssetsLibrary, Image);
			else
				_annotator.SetTag(AssetsLibrary, Image, value);
		}
	}

	Named? TagSelectionToolingDataContext.SelectedTag
	{
		get => SelectedTag;
		set => SelectedTag = (Tag?)value;
	}

	[ObservableProperty, NotifyPropertyChangedFor(nameof(SelectedTag), nameof(IsEnabled))]
	public partial Image? Image { get; set; }

	public bool IsEnabled => Image != null;

	public ClassifierAnnotationViewModel(ClassifierAnnotator annotator, ImagesViewModel imagesViewModel)
	{
		_annotator = annotator;
		_disposable = imagesViewModel.SelectedImageChanged.Subscribe(_ => Image = imagesViewModel.SelectedImage);
		Image = imagesViewModel.SelectedImage;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly ClassifierAnnotator _annotator;
	private AssetsLibrary<ClassifierAsset>? AssetsLibrary => DataSet?.AssetsLibrary;
	private readonly IDisposable _disposable;

	private ClassifierAsset? Asset =>
		Image == null ? null : AssetsLibrary?.Assets.GetValueOrDefault(Image);
}