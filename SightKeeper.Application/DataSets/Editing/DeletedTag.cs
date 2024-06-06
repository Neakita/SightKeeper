using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class DeletedTag
{
    public Tag Tag { get; }
    public DeletedTagAction? Action { get; }

    public DeletedTag(Tag tag, DeletedTagAction? action = null)
    {
	    Tag = tag;
        Action = action;
    }
}