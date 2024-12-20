﻿using MemoryPack;
using SightKeeper.Application;

namespace SightKeeper.Data;

// In fact, it is not required to implement this interface,
// but this way you can automatically delegate the implementation of this interface
// to another class that contains this class as a member, via IDE's tools
// and from a logical point of view there are no contradictions.
[MemoryPackable]
public sealed partial class ApplicationSettings : ApplicationSettingsProvider
{
	public bool CustomDecorations { get; set; }
}