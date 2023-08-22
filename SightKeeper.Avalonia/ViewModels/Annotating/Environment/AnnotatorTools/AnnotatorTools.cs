using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface AnnotatorTools
{
    void ScrollItemClass(bool reverse);
}

public interface AnnotatorTools<TAsset> : AnnotatorTools where TAsset : Asset
{
    DataSetViewModel<TAsset>? DataSetViewModel { get; set; }
}