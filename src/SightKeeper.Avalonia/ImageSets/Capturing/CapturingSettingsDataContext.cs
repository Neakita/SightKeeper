using System;
using Sightful.Avalonia.Controls.GestureBox;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

public interface CapturingSettingsDataContext
{
	ushort MaximumWidth { get; }
	ushort MaximumHeight { get; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	double? FrameRateLimit { get; set; }
	ushort? UnusedImagesLimit { get; set; }
	object? Gesture { get; set; }
	IObservable<GestureEdit> GestureEditsObservable { get; }
}