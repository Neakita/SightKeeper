using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;

namespace SightKeeper.Tests.Common;

public static class DomainTestsHelper
{
    public static DataSet NewDataSet => new("Test data set");
}