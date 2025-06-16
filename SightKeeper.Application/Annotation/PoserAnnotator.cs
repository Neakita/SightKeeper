using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Annotation;

public interface PoserAnnotator
{
	void CreateKeyPoint(DomainPoserItem item, DomainTag tag, Vector2<double> position);
	void SetKeyPointPosition(DomainKeyPoint keyPoint, Vector2<double> position);
	void SetKeyPointVisibility(DomainKeyPoint3D keyPoint, bool isVisible);
	void DeleteKeyPoint(DomainPoserItem item, DomainTag tag);
}