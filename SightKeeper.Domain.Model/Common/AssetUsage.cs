﻿namespace SightKeeper.Domain.Model.Common;

[Flags]
public enum AssetUsage
{
	None,
    Train = 1 << 0,
    Validation = 1 << 1,
    Test = 1 << 2,
    Any = Train | Validation | Test
}