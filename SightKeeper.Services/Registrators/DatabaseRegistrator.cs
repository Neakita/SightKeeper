using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SightKeeper.DAL;

namespace SightKeeper.Services.Registrators;

public static class DatabaseRegistrator
{
	public static IServiceCollection AddDatabase(this IServiceCollection services) => services.AddTransient<DbContext, AppDbContext>();
}