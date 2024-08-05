using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierTag : Tag
{
	public IReadOnlyCollection<ClassifierAsset> Assets => _assets;
	public ClassifierTagsLibrary Library { get; }
	public ClassifierDataSet DataSet => Library.DataSet;

	internal ClassifierTag(string name, ClassifierTagsLibrary library) : base(name, library)
	{
		Library = library;
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

	protected override IEnumerable<Tag> Siblings => Library;
	private readonly HashSet<ClassifierAsset> _assets;
}