using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class KeyPointTag
{
	public string Name
	{
		get => _name;
		[MemberNotNull(nameof(_name))] set
		{
			if (_name == value)
				return;
			foreach (var sibling in PoserTag.KeyPoints)
				Guard.IsNotEqualTo(sibling.Name, value);
			_name = value;
		}
	}

	public PoserTag PoserTag { get; }

	internal KeyPointTag(string name, PoserTag poserTag)
	{
		PoserTag = poserTag;
		Name = name;
	}

	private string _name;
}