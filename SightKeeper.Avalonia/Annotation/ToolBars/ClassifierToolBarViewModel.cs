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
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

public sealed partial class ClassifierToolBarViewModel : ToolBarViewModel, IDisposable
{
	[ObservableProperty, NotifyPropertyChangedFor(nameof(Tags))]
	public partial ClassifierDataSet? DataSet { get; set; }
	public IReadOnlyCollection<Tag> Tags => DataSet?.TagsLibrary.Tags ?? ReadOnlyCollection<Tag>.Empty;

	public Tag? Tag
	{
		get => Asset?.Tag;
		set
		{
			Guard.IsNotNull(AssetsLibrary);
			Guard.IsNotNull(Screenshot);
			if (value == null)
				_annotator.DeleteAsset(AssetsLibrary, Screenshot);
			else
				_annotator.SetTag(AssetsLibrary, Screenshot, value);
		}
	}

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Tag))]
	public partial Screenshot? Screenshot { get; set; }

	public ClassifierToolBarViewModel(ClassifierAnnotator annotator, ScreenshotsViewModel screenshotsViewModel)
	{
		_annotator = annotator;
		_disposable = screenshotsViewModel.SelectedScreenshotChanged.Subscribe(screenshot => Screenshot = screenshot?.Value);
		Screenshot = screenshotsViewModel.SelectedScreenshot?.Value;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly ClassifierAnnotator _annotator;
	private AssetsLibrary<ClassifierAsset>? AssetsLibrary => DataSet?.AssetsLibrary;
	private readonly IDisposable _disposable;

	private ClassifierAsset? Asset =>
		Screenshot == null ? null : AssetsLibrary?.Assets.GetValueOrDefault(Screenshot);
}