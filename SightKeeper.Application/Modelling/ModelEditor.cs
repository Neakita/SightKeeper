using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application.Modelling;

public interface ModelEditor
{
    public void ApplyChanges(Model model, ModelChanges changes);
}