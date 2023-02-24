using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Classifier;

[Table("ClassifierModels")]
public sealed class ClassifierModel : Abstract.Model
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