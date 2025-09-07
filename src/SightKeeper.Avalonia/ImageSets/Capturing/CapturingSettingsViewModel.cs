using System;
using HotKeys;
using Sightful.Avalonia.Controls.GestureBox;
using SightKeeper.Application;
using SightKeeper.Application.ScreenCapturing;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

public sealed class CapturingSettingsViewModel(ScreenBoundsProvider screenBoundsProvider, ImageCapturer capturer, ObservableSharpHookGesture observableGesture)
	: ViewModel, CapturingSettingsDataContext
{
	public ushort MaximumWidth => screenBoundsProvider.MainScreenSize.X;
	public ushort MaximumHeight => screenBoundsProvider.MainScreenSize.Y;

	public ushort Width
	{
		get => capturer.ImageSize.X;
		set
		{
			OnPropertyChanging();
			capturer.ImageSize = capturer.ImageSize with { X = value };
			OnPropertyChanged();
		}
	}

	public ushort Height
	{
		get => capturer.ImageSize.Y;
		set
		{
			OnPropertyChanging();
			capturer.ImageSize = capturer.ImageSize with { Y = value };
			OnPropertyChanged();
		}
	}

	public double? FrameRateLimit
	{
		get => capturer.FrameRateLimit;
		set
		{
			OnPropertyChanging();
			capturer.FrameRateLimit = value;
			OnPropertyChanged();
		}
	}

	public object? Gesture
	{
		get => capturer.Gesture;
		set => capturer.Gesture = (Gesture?)value ?? HotKeys.Gesture.Empty;
	}

	public IObservable<GestureEdit> GestureEditsObservable => observableGesture;
}