using SightKeeper.Backend.Data;

namespace SightKeeper.Tests;

public class DbContextTests
{
	[Fact]
	public void CanCreateDbContext()
	{
		// assign
		using var dbContext = new AppDbContext();
		dbContext.Database.EnsureDeleted();
		
		// act
		bool created = dbContext.Database.EnsureCreated();
		
		// assert
		Assert.True(created);
		
		// clean-up
		dbContext.Database.EnsureDeleted();
	}
}