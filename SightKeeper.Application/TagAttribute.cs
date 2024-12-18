namespace SightKeeper.Application;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class TagAttribute : Attribute
{
	public object Tag { get; }

	public TagAttribute(object tag)
	{
		Tag = tag;
	}
}