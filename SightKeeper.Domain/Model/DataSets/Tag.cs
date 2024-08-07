using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class Tag
{
	public string Name
	{
		get => _name;
		set => SetName(value, Siblings);
	}

	public uint Color { get; set; }
	
	public abstract DataSet DataSet { get; }
	
	internal abstract bool CanDelete { get; }

	/// <param name="name">Initial name</param>
	/// <param name="siblings">A collection of siblings for initial name validation</param>
	internal Tag(string name, IEnumerable<Tag> siblings)
	{
		SetName(name, siblings);
	}

	protected abstract IEnumerable<Tag> Siblings { get; }

	private string _name;

	[MemberNotNull(nameof(_name))]
	private void SetName(string value, IEnumerable<Tag> siblings)
	{
		if (_name == value)
			return;
		foreach (var sibling in siblings)
			Guard.IsNotEqualTo(value, sibling.Name);
		_name = value;
	}
}