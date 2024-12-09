using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal sealed partial class ClassifierToolBarViewModel : ToolBarViewModel<ClassifierAssetViewModel, ClassifierAsset>
{
	public IReadOnlyCollection<Tag> Tags { get; }

	public Tag? Tag
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public ClassifierToolBarViewModel(IReadOnlyCollection<Tag> tags, ClassifierAnnotator annotator)
	{
		_annotator = annotator;
		Tags = tags;
	}

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Tag))]
	private ScreenshotViewModel? _screenshot;

	private readonly ClassifierAnnotator _annotator;
}