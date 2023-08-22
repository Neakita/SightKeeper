using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface AnnotatorWorkSpace
{
}

public interface AnnotatorWorkSpace<TAsset> : AnnotatorWorkSpace where TAsset : Asset
{
    DataSetViewModel<TAsset>? DataSetViewModel { get; set; }
}