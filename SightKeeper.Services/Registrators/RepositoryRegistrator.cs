using Microsoft.Extensions.DependencyInjection;
using SightKeeper.DAL.Members.Common;
using SightKeeper.DAL.Members.Detector;
using SightKeeper.DAL.Repositories;

namespace SightKeeper.Services.Registrators;

public static class RepositoryRegistrator
{
	public static IServiceCollection AddDbRepositories(this IServiceCollection services) => services
		.AddTransient<IRepository<DetectorModel>, EFRepository<DetectorModel>>()
		.AddTransient<IRepository<Game>, EFRepository<Game>>();
}