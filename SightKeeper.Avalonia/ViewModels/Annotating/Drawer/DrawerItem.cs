﻿using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

internal interface DrawerItem
{
    bool IsDashed { get; }
    BoundingViewModel Bounding { get; }
    ItemClass ItemClass { get; }
}