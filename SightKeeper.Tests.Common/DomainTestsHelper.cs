using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Tests.Common;

public static class DomainTestsHelper
{
    public static DataSet<DetectorAsset> NewDetectorDataSet => new("Test data set");
}