using FluentAssertions;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Services;
using SightKeeper.Tests.Common;

namespace SightKeeper.Services.Tests;

public sealed class InheritedGenericRepositoryTests
{
	[Fact]
	public void ShouldAddAndGetSameItem()
	{
		var repository = new InheritedGenericRepository<DetectorModel, Model>(
			new GenericDynamicDbRepository<Model>(new TestDbContextFactory()));

		DetectorModel model = new("Test model");
		
		repository.Add(model);
		repository.Items.Single().Should().BeSameAs(model);
	}

	[Fact]
	public void ShouldGetItemFromBaseRepository()
	{
		var baseRepository = new GenericDynamicDbRepository<Model>(new TestDbContextFactory());
		var repository = new InheritedGenericRepository<DetectorModel, Model>(baseRepository);

		DetectorModel model = new("Test model");
		
		baseRepository.Add(model);
		repository.Items.Single().Should().BeSameAs(model);
	}
}
