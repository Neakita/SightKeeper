using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Tests.Common;

public abstract class DbRelatedTests
{
	protected readonly AppDbContextFactory DbContextFactory = new TestDbContextFactory();
}