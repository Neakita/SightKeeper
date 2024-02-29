using System.Diagnostics.CodeAnalysis;
using SightKeeper.Domain.Model.DataSet;

namespace SightKeeper.Application.Training;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "NotAccessedField.Local")]
public readonly struct DataSetConfigurationParameters
{
    public readonly string path;
    public readonly string train;
    public readonly string val;
    public readonly Dictionary<byte, string> names;

    public DataSetConfigurationParameters(string path, IEnumerable<ItemClass> itemClasses, string train = "images", string val = "images")
    {
        this.path = path;
        this.train = train;
        this.val = val;
        names = itemClasses.Select((itemClass, index) => (name: itemClass.Name, index)).ToDictionary(tuple => (byte)tuple.index, tuple => tuple.name);
    }
}