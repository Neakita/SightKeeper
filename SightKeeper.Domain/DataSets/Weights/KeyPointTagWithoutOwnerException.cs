using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class KeyPointTagWithoutOwnerException : Exception
{
	private const string DefaultMessage =
		"A key point tag was specified, but not its poser tag owner. " +
		"Either pass its poser tag owner, or don't pass the key point tag";

	public Tag KeyPointTag { get; }
	public DomainPoserTag ExpectedOwner { get; }

	public KeyPointTagWithoutOwnerException(Tag keyPointTag, DomainPoserTag expectedOwner) : base(DefaultMessage)
	{
		KeyPointTag = keyPointTag;
		ExpectedOwner = expectedOwner;
	}
}