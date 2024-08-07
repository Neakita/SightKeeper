using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierTag : Tag, MinimumTagsCount, TagsFactory<ClassifierTag>
{
	public static byte MinimumCount => 2;

	static ClassifierTag TagsFactory<ClassifierTag>.Create(string name, TagsLibrary<ClassifierTag> library)
	{
		return new ClassifierTag(name, library);
	}

	public IReadOnlyCollection<ClassifierAsset> Assets => _assets;
	public TagsLibrary<ClassifierTag> Library { get; }
	public override DataSet DataSet => Library.DataSet;
	internal override bool CanDelete => _assets.Count == 0;

	internal ClassifierTag(string name, TagsLibrary<ClassifierTag> library) : base(name, library)
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