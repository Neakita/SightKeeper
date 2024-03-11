using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Tests.Common;

public static class DomainTestsHelper
{
    public static DataSet NewDataSet => new("Test data set");
}