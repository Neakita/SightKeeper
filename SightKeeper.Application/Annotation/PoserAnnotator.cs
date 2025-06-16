using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Annotation;

public interface PoserAnnotator
{
	void CreateKeyPoint(PoserItem item, DomainTag tag, Vector2<double> position);
	void SetKeyPointPosition(KeyPoint keyPoint, Vector2<double> position);
	void SetKeyPointVisibility(KeyPoint3D keyPoint, bool isVisible);
	void DeleteKeyPoint(PoserItem item, DomainTag tag);
}