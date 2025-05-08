using System.Reactive.Linq;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public sealed class MergeObservableAnnotator : ObservableAnnotator
{
	public IObservable<Image> AssetsChanged { get; }

	public MergeObservableAnnotator(params IEnumerable<ObservableAnnotator> observableAnnotators)
	{
		AssetsChanged = observableAnnotators.Select(annotator => annotator.AssetsChanged).Merge();
	}
}