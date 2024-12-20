using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public ClassifierDataSet? DataSet
	{
		get;
		set
		{
			field = value;
			_assetsLibrary = value?.AssetsLibrary;
			Tags = value?.TagsLibrary.Tags ?? ReadOnlyCollection<Tag>.Empty;
		}
	}

	[ObservableProperty]
	public partial IReadOnlyCollection<Tag> Tags { get; private set; } = ReadOnlyCollection<Tag>.Empty;

	public Tag? Tag
	{
		get => Asset?.Tag;
		set
		{
			Guard.IsNotNull(_assetsLibrary);
			Guard.IsNotNull(Screenshot);
			var screenshot = Screenshot.Value;
			if (value == null)
				_annotator.DeleteAsset(_assetsLibrary, screenshot);
			else
				_annotator.SetTag(_assetsLibrary, screenshot, value);
		}
	}

	public ClassifierToolBarViewModel(ClassifierAnnotator annotator, ScreenshotsViewModel screenshotsViewModel)
	{
		_annotator = annotator;
		_disposable = screenshotsViewModel.SelectedScreenshotChanged.Subscribe(screenshot => Screenshot = screenshot);
        Screenshot = screenshotsViewModel.SelectedScreenshot;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Tag))]
	internal partial ScreenshotViewModel? Screenshot { get; set; }

	private readonly ClassifierAnnotator _annotator;
	private AssetsLibrary<ClassifierAsset>? _assetsLibrary;
	private readonly IDisposable _disposable;

    private ClassifierAsset? Asset => Screenshot == null ? null : _assetsLibrary?.Assets.GetValueOrDefault(Screenshot.Value);
}