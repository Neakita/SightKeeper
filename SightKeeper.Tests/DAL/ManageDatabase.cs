using System.Reflection;
using SightKeeper.DAL;
using Xunit.Sdk;

namespace SightKeeper.Tests.DAL;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal sealed class ManageDatabase : BeforeAfterTestAttribute
{
	public override void Before(MethodInfo methodUnderTest)
	{
		using AppDbContext dbContext = Helper.NewDbContext;
		dbContext.Database.EnsureCreated();
	}

	public override void After(MethodInfo methodUnderTest)
	{
		using AppDbContext dbContext = Helper.NewDbContext;
		dbContext.Database.EnsureDeleted();
	}
}