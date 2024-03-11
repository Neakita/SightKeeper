using SightKeeper.Domain.Model.DataSets;
using RectangleF = System.Drawing.RectangleF;

namespace SightKeeper.Application.Prediction;

public readonly struct DetectionItem
{
    public readonly ItemClass ItemClass;
    public readonly RectangleF Bounding;
    public readonly float Probability;

    public DetectionItem(ItemClass itemClass, RectangleF bounding, float probability)
    {
        ItemClass = itemClass;
        Bounding = bounding;
        Probability = probability;
    }

    public override string ToString() => $"{nameof(ItemClass)}: {ItemClass}, {nameof(Bounding)}: {Bounding}, {nameof(Probability)}: {Probability}";
}