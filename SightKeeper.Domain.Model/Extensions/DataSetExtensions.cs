using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Extensions;

public static class DataSetExtensions
{
    public static ModelType GetModelType(this DataSet dataSet) => dataSet switch
    {
        DataSet<DetectorAsset> => ModelType.Detector,
        _ => ThrowHelper.ThrowArgumentOutOfRangeException<ModelType>(nameof(dataSet), dataSet.GetType(), "Unexpected data set model type")
    };
}