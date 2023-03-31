using FluentAssertions;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Infrastructure.Services.Tests;

public class GenericDynamicDbRepositoryTests
{
	[Fact]
	public void ShouldAddAndGetSameInstance()
	{
		GenericDynamicDbRepository<Model> repository = GetRepository<Model>();
		DetectorModel model = new("Test model");
		repository.Add(model);
		repository.Items.Single().Should().BeSameAs(model);
	}

	private GenericDynamicDbRepository<TEntity> GetRepository<TEntity>() where TEntity : class, Entity =>
		new(new TestDbContextFactory());
}