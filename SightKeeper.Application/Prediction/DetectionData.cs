using System.Collections.Immutable;

namespace SightKeeper.Application.Prediction;

public sealed record DetectionData(byte[] Image, ImmutableList<DetectionItem> Items);