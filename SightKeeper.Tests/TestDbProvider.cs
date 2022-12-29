using Microsoft.EntityFrameworkCore;
using SightKeeper.DAL;

namespace SightKeeper.Tests;

public sealed class TestDbProvider : IAppDbProvider
{
	public AppDbContext NewContext => new(_options);


	public TestDbProvider()
	{
		_options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
	}

	
	private readonly DbContextOptions<AppDbContext> _options;
}
