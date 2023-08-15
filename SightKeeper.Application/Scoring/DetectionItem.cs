﻿using System.Drawing;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Scoring;

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
}