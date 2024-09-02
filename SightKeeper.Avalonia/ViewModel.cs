using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Avalonia;

internal abstract class ViewModel : ObservableObject
{
    protected void OnPropertiesChanging(IEnumerable<string> properties)
    {
        foreach (var property in properties)
            OnPropertyChanging(property);
    }
    
    protected void OnPropertiesChanged(IEnumerable<string> properties)
    {
        foreach (var property in properties)
            OnPropertyChanged(property);
    }
}