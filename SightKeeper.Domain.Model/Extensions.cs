using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model;

public static class Extensions
{
    public static ModelType GetDomainType(this Model.Model model) => model switch
    {
        DetectorModel => ModelType.Detector,
        _ => ThrowHelper.ThrowArgumentOutOfRangeException<ModelType>("model", $"Unexpected model type: {model.GetType()}")
    };
}