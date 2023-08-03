using System.Collections.Generic;
using System.Threading.Tasks;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface IAnnotatingViewModel
{
    Task<IReadOnlyCollection<Model>> Models { get; }
    Model? SelectedModel { get; set; }
    bool CanChangeSelectedModel { get; }
    AnnotatorScreenshotsViewModel Screenshots { get; }
    ScreenshoterViewModel Screenshoter { get; }
    AnnotatorTools? Tools { get; }
    AnnotatorWorkSpace? WorkSpace { get; }
}