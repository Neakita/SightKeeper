using SightKeeper.Domain.Model.Model;

namespace SightKeeper.Application.Modelling;

public interface ModelEditor
{
    public void ApplyChanges(Model model, ModelChanges changes);
}