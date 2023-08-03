using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface AnnotatorTools
{
}

public interface AnnotatorTools<TModel> : AnnotatorTools where TModel : Model
{
}