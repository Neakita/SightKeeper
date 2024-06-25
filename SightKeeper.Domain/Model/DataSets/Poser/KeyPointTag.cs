using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class KeyPointTag : Tag
{
	public override string Name
	{
		get => _name;
		set
		{
			if (_name == value)
				return;
			Guard.IsFalse(PoserTag.KeyPoints.Any(tag => tag.Name == value));
			_name = value;
		}
	}

	public PoserTag PoserTag { get; }

	internal KeyPointTag(string name, uint color, PoserTag poserTag)
	{
		_name = name;
		Color = color;
		PoserTag = poserTag;
	}

	private string _name;
}