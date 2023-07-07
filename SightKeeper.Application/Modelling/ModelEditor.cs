using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Modelling;

public interface ModelEditor
{
    public void ApplyChanges(Model model, ModelChanges changes);
}