using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierTag : Tag
{
	public override string Name
	{
		get => _name;
		set
		{
			if (_name == value)
				return;
			foreach (var sibling in Library)
				Guard.IsNotEqualTo(value, sibling.Name);
			_name = value;
		}
	}

	public ClassifierTagsLibrary Library { get; }
	public ClassifierDataSet DataSet => Library.DataSet;

	internal ClassifierTag(string name, ClassifierTagsLibrary library)
	{
		_name = name;
		Library = library;
	}

	private string _name;
}