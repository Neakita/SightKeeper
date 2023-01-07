using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL.Domain.Classifier;

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