using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Annotation;

public interface PoserAnnotator
{
	void CreateKeyPoint(PoserItem item, Tag tag, Vector2<double> position);
	void SetKeyPointPosition(KeyPoint keyPoint, Vector2<double> position);
	void DeleteKeyPoint(PoserItem item, Tag tag);
}