using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface AnnotatorTools
{
    void ScrollItemClass(bool reverse);
}

public interface AnnotatorTools<TModel> : AnnotatorTools where TModel : Model
{
}