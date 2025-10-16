using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

internal sealed class EnsureCanDeleteImageSetRepository(Repository<ImageSet> inner) : Repository<ImageSet>
{
	public IReadOnlyCollection<ImageSet> Items => inner.Items;

	public IObservable<ImageSet> Added => inner.Added;

	public IObservable<ImageSet> Removed => inner.Removed;

	public void Add(ImageSet set)
	{
		inner.Add(set);
	}

	public void Remove(ImageSet set)
	{
		var canDelete = set.CanDelete();
		Guard.IsTrue(canDelete);
		inner.Remove(set);
	}

	public void Dispose()
	{
		inner.Dispose();
	}
}