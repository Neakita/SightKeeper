using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Tests.Common;

public abstract class DbRelatedTests
{
	protected readonly IAppDbContextFactory DbContextFactory = new TestDbContextFactory();
}