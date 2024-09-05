using System.Diagnostics.CodeAnalysis;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class ItemProperty
{
	public string Name
	{
		get => _name;
		[MemberNotNull(nameof(_name))]
		set => _name = value;
	}

	protected ItemProperty(string name)
	{
		Name = name;
	}

	private string _name;
}