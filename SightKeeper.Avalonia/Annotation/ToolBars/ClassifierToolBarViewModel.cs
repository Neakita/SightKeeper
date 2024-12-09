using System.Collections.Generic;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal sealed class ClassifierToolBarViewModel : ToolBarViewModel<ClassifierAssetViewModel, ClassifierAsset>
{
	public IReadOnlyCollection<ClassifierTag> Tags { get; }

	public ClassifierTag? Tag
	{
		get => Screenshot?.Asset?.Tag;
		set
		{
			Guard.IsNotNull(Screenshot);
			_annotator.SetTag(Screenshot.Value, value);
			Screenshot.UpdateAsset();
			Screenshot.Asset?.NotifyTagChanged();
		}
	}

	public ScreenshotViewModel<ClassifierAssetViewModel, ClassifierAsset>? Screenshot
	{
		get;
		set
		{
			if (field == value)
				return;
			OnPropertyChanging();
			OnPropertyChanging(nameof(Tag));
			field = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Tag));
		}
	}

	public ClassifierToolBarViewModel(IReadOnlyCollection<ClassifierTag> tags, ClassifierAnnotator annotator)
	{
		_annotator = annotator;
		Tags = tags;
	}

	private readonly ClassifierAnnotator _annotator;
}