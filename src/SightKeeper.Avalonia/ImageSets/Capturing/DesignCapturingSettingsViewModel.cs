using System;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Sightful.Avalonia.Controls.GestureBox;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

internal sealed partial class DesignCapturingSettingsViewModel : ViewModel, CapturingSettingsDataContext
{
	public ushort MaximumWidth => 1920;
	public ushort MaximumHeight => 1080;

	[ObservableProperty] public partial ushort Width { get; set; } = 320;
	[ObservableProperty] public partial ushort Height { get; set; } = 320;
	[ObservableProperty] public partial double? FrameRateLimit { get; set; }
	public ushort? UnusedImagesLimit { get; set; }

	public object? Gesture { get; set; }
	public IObservable<GestureEdit> GestureEditsObservable { get; } = Observable.Empty<GestureEdit>();
}