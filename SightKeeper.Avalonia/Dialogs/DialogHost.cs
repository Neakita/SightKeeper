using System.ComponentModel;

namespace SightKeeper.Avalonia.Dialogs;

internal interface DialogHost : INotifyPropertyChanged
{
	DialogManager DialogManager { get; }
}