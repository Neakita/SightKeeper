using System.Collections.Generic;
using System.Threading.Tasks;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public interface IAnnotatingViewModel
{
    Task<IReadOnlyCollection<Model>> Models { get; }
    Model? SelectedModel { get; set; }
    bool CanChangeSelectedModel { get; }
    IReadOnlyCollection<Screenshot> Screenshots { get; }
    ScreenshoterViewModel Screenshoter { get; }
}