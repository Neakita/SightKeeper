using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface AnnotatorWorkSpace
{
}

public interface AnnotatorWorkSpace<TModel> : AnnotatorWorkSpace where TModel : Model
{
}