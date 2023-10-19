using System.Collections.Immutable;

namespace SightKeeper.Application.Prediction;

public record DetectionData(byte[] Image, ImmutableList<DetectionItem> Items);