using SightKeeper.Application.Prediction;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving;

public record MouseMovingContext(DetectionData DetectionData, DetectionItem TargetItem);