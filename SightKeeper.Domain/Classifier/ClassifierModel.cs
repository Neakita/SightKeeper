using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Abstract;
using SightKeeper.Domain.Common;

namespace SightKeeper.Domain.Classifier;

[Table("ClassifierModels")]
public sealed class ClassifierModel : Model
{
	public ClassifierModel(string name) : base(name)
	{
	}

	public ClassifierModel(string name, Resolution resolution) : base(name, resolution)
	{
	}

	public ClassifierModel(int id, string name) : base(id, name)
	{
	}
}