using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal abstract partial class EditableTagViewModel : ViewModel, EditableTagDataContext
{
	[ObservableProperty] public partial string Name { get; set; } = string.Empty;
	[ObservableProperty] public partial Color Color { get; set; }
}