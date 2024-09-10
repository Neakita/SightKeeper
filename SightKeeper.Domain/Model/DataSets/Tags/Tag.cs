using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Tags;

public abstract class Tag
{
	public string Name
	{
		get => _name;
		set => SetName(value, Siblings);
	}

	public uint Color { get; set; }
	public abstract DataSet DataSet { get; }
	public abstract bool IsInUse { get; }

	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return Equals((Tag)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Name, Color, DataSet);
	}

	public abstract void Delete();

	/// <param name="name">Initial name</param>
	/// <param name="siblings">A collection of siblings for initial name validation</param>
	internal Tag(string name, IEnumerable<Tag> siblings)
	{
		SetName(name, siblings);
	}

	protected abstract IEnumerable<Tag> Siblings { get; }

	protected bool Equals(Tag other)
	{
		return Name == other.Name && Color == other.Color && DataSet.Equals(other.DataSet);
	}

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