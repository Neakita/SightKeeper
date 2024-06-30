using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierTag : Tag
{
	public override string Name
	{
		get => _name;
		[MemberNotNull(nameof(_name))] set
		{
			if (_name == value)
				return;
			foreach (var sibling in Library)
				Guard.IsNotEqualTo(value, sibling.Name);
			_name = value;
		}
	}

	public IReadOnlyCollection<ClassifierAsset> Assets => _assets;
	public override ClassifierTagsLibrary Library { get; }
	public override ClassifierDataSet DataSet => Library.DataSet;

	internal ClassifierTag(string name, ClassifierTagsLibrary library)
	{
		Library = library;
		Name = name;
		_assets = new HashSet<ClassifierAsset>();
	}

	internal void AddAsset(ClassifierAsset asset)
	{
		Guard.IsTrue(_assets.Add(asset));
	}

	internal void RemoveAsset(ClassifierAsset asset)
	{
		Guard.IsTrue(_assets.Remove(asset));
	}

	private readonly HashSet<ClassifierAsset> _assets;
	private string _name;
}