using Microsoft.EntityFrameworkCore;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Tests.Common;

public sealed class TestDbContextFactory : IAppDbContextFactory
{
	private readonly DbContextOptions<AppDbContext> _options;
	
	public TestDbContextFactory() =>
		_options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
	
	public AppDbContext CreateDbContext() => new(_options);
}