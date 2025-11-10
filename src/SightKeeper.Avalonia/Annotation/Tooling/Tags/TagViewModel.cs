using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Tags;

internal sealed class TagViewModel : TagDataContext
{
	public Tag Tag { get; }
	public string Name => Tag.Name;

	public TagViewModel(Tag tag)
	{
		Tag = tag;
	}

	public override bool Equals(object? obj)
	{
		return ReferenceEquals(this, obj) || obj is TagViewModel other && Equals(other);
	}

	public override int GetHashCode()
	{
		return Tag.GetHashCode();
	}

	private bool Equals(TagViewModel other)
	{
		return Tag.Equals(other.Tag);
	}
}