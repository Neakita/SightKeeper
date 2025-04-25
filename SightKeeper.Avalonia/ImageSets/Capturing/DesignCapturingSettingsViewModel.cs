using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

internal sealed partial class DesignCapturingSettingsViewModel : ViewModel, CapturingSettingsDataContext
{
	public ushort MaximumWidth => 1920;
	public ushort MaximumHeight => 1080;

	[ObservableProperty] public partial ushort Width { get; set; } = 320;
	[ObservableProperty] public partial ushort Height { get; set; } = 320;
	[ObservableProperty] public partial double? FrameRateLimit { get; set; }
}