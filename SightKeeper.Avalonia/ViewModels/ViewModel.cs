using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Avalonia.ViewModels;

public class ViewModel : ObservableObject
{
    protected void OnPropertiesChanging(params string[] properties)
    {
        foreach (var property in properties)
            OnPropertyChanging(property);
    }
    
    protected void OnPropertiesChanged(params string[] properties)
    {
        foreach (var property in properties)
            OnPropertyChanged(property);
    }
}