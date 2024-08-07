using SightKeeper.Domain.Model.DataSets.Tags;
using RectangleF = System.Drawing.RectangleF;

namespace SightKeeper.Application.Prediction;

public readonly struct DetectionItem
{
    public readonly Tag Tag;
    public readonly RectangleF Bounding;
    public readonly float Probability;

    public DetectionItem(Tag tag, RectangleF bounding, float probability)
    {
        Tag = tag;
        Bounding = bounding;
        Probability = probability;
    }

    public override string ToString() => $"{nameof(Tag)}: {Tag}, {nameof(Bounding)}: {Bounding}, {nameof(Probability)}: {Probability}";
}