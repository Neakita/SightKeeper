using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Application.DataSets;

public sealed class TagInfo
{
    public string Name { get; }
    public uint Color { get; }

    public TagInfo(string name, uint color)
    {
        Name = name;
        Color = color;
    }

    public TagInfo(Tag tag)
    {
        Name = tag.Name;
        Color = tag.Color;
    }
}