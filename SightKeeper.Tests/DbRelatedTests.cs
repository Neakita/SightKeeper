using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Tests;

public abstract class DbRelatedTests
{
	protected readonly IAppDbContextFactory DbContextFactory = new TestDbContextFactory();
}