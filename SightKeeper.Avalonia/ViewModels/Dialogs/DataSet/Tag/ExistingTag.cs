using Avalonia.Media;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.Tag;

internal sealed class ExistingTag : ViewModel, EditableTag
{
    public Domain.Model.DataSets.Tag Tag { get; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public bool IsEdited => Name != Tag.Name || Color.ToUInt32() != Tag.Color;

    public ExistingTag(Domain.Model.DataSets.Tag tag)
    {
        Tag = tag;
        Name = tag.Name;
        Color = Color.FromUInt32(tag.Color);
    }
    
    public TagInfo ToTagInfo() => new(Name, Color.ToUInt32());
    public EditedTag ToEditedTag() => new EditedTag(Tag, Name, Color.ToUInt32());
}