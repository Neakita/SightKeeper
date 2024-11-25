using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal abstract partial class ScreenshotsViewModel : ViewModel
{
	public abstract ScreenshotsLibrary Library { get; }
	public abstract IReadOnlyCollection<ScreenshotViewModel> Screenshots { get; }
	public abstract IReadOnlyCollection<DateOnly> Dates { get; }
	public ScreenshotImageLoader ImageLoader { get; }

	protected ScreenshotsViewModel(ScreenshotImageLoader imageLoader)
	{
		ImageLoader = imageLoader;
	}

	[ObservableProperty] private ScreenshotViewModel? _selectedScreenshot;
}