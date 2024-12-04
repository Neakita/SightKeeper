namespace SightKeeper.Domain.Model.DataSets.Tags;

public abstract class TagsUsageProvider
{
	public abstract bool IsInUse(Tag tag);
}