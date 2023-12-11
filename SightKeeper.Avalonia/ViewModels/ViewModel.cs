using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Avalonia.ViewModels;

public abstract class ViewModel : ObservableObject
{
    protected void OnPropertiesChanging(IEnumerable<string> otherProperties)
    {
        foreach (var property in otherProperties)
            OnPropertyChanging(property);
    }
    
    protected void OnPropertiesChanged(IEnumerable<string> otherProperties)
    {
        foreach (var property in otherProperties)
            OnPropertyChanged(property);
    }
}