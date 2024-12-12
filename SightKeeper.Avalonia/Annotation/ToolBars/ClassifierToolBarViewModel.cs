using System;
using System.Collections.Generic;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal sealed partial class ClassifierToolBarViewModel : ToolBarViewModel, IDisposable
{
	public IReadOnlyCollection<Tag> Tags { get; }

	public Tag? Tag
	{
		get => Asset?.Tag;
		set
		{
			Guard.IsNotNull(Screenshot);
			var screenshot = Screenshot.Value;
			if (value == null)
				_annotator.DeleteAsset(_assetsLibrary, screenshot);
			else
				_annotator.SetTag(_assetsLibrary, screenshot, value);
		}
	}

	public ClassifierToolBarViewModel(ClassifierDataSet dataSet, ClassifierAnnotator annotator, ScreenshotsViewModel screenshotsViewModel)
	{
		_annotator = annotator;
		_assetsLibrary = dataSet.AssetsLibrary;
		Tags = dataSet.TagsLibrary.Tags;
		_disposable = screenshotsViewModel.SelectedScreenshotChanged.Subscribe(screenshot => Screenshot = screenshot);
		_screenshot = screenshotsViewModel.SelectedScreenshot;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}


	private readonly ClassifierAnnotator _annotator;
	private readonly AssetsLibrary<ClassifierAsset> _assetsLibrary;
	private readonly IDisposable _disposable;
	[ObservableProperty, NotifyPropertyChangedFor(nameof(Tag))]
	private ScreenshotViewModel? _screenshot;
	private ClassifierAsset? Asset => Screenshot == null ? null : _assetsLibrary.Assets.GetValueOrDefault(Screenshot.Value);
}