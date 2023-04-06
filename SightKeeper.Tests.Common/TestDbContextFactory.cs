using Microsoft.EntityFrameworkCore;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Tests.Common;

public sealed class TestDbContextFactory : AppDbContextFactory
{
	public TestDbContextFactory() =>
		_options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
	
	public AppDbContext CreateDbContext() => new(_options);
	
	private readonly DbContextOptions<AppDbContext> _options;
}