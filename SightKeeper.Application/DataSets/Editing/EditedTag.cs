using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class EditedTag
{
    public Tag Tag { get; }
    public string Name { get; }
    public uint Color { get; }

    public EditedTag(Tag tag, string name, uint color)
    {
        Tag = tag;
        Name = name;
        Color = color;
    }
}