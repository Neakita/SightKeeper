using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;

namespace SightKeeper.Avalonia.Misc;

public sealed class MainWindowActivityService : SelfActivityService
{
    public bool IsOwnWindowActive => MainWindow.IsActive;

    private Window MainWindow
    {
        get
        {
            var application = global::Avalonia.Application.Current;
            Guard.IsNotNull(application);
            var applicationLifetime = (ClassicDesktopStyleApplicationLifetime?)application.ApplicationLifetime;
            Guard.IsNotNull(applicationLifetime);
            var window = applicationLifetime.MainWindow;
            Guard.IsNotNull(window);
            return window;
        }
    }
}