using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class RemovedTag : ExistingTagData
{
	public Tag Tag { get; }

	public RemovedTag(Tag tag)
	{
		Tag = tag;
	}
}