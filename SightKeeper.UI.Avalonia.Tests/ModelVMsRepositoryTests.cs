using FluentAssertions;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Services;
using SightKeeper.Tests.Common;
using SightKeeper.UI.Avalonia.ViewModels.Components;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.Tests;

public sealed class ModelVMsRepositoryTests
{
	[Fact]
	public void ShouldGetSameInstanceAfterAdd()
	{
		ModelVMsRepository repository = Repository;
		DetectorModelViewModel testModel = new(new DetectorModel("Test model"));
		repository.Add(testModel);
		repository.Items.Single().Should().BeSameAs(testModel);
	}

	[Fact]
	public void ShouldAddInstanceFromInnerRepository()
	{
		var innerRepository = new GenericDynamicDbRepository<Model>(new TestDbContextFactory());
		ModelVMsRepository repository = new(innerRepository);
		DetectorModel testModel = new("Test model");
		innerRepository.Add(testModel);
		repository.Items.Should().ContainSingle(modelVM => modelVM.Model == testModel);
	}

	[Fact]
	public void ShouldDeleteModelVMFromModelRepository()
	{
		var innerRepository = new GenericDynamicDbRepository<Model>(new TestDbContextFactory());
		ModelVMsRepository repository = new(innerRepository);
		DetectorModel testModel = new("Test model");
		DetectorModelViewModel testModelVM = new(testModel);
		repository.Add(testModelVM);
		repository.Items.Single().Should().BeSameAs(testModelVM);
		
		innerRepository.Remove(testModel);
		repository.Items.Should().BeEmpty();
	}

	[Fact]
	public void ShouldDeleteModelFromModelVMRepository()
	{
		var innerRepository = new GenericDynamicDbRepository<Model>(new TestDbContextFactory());
		ModelVMsRepository repository = new(innerRepository);
		DetectorModel testModel = new("Test model");
		innerRepository.Add(testModel);
		repository.Items.Should().ContainSingle(modelVM => modelVM.Model == testModel);
		repository.Remove(repository.Items.Single());
		innerRepository.Items.Should().BeEmpty();
	}
	
	private static ModelVMsRepository Repository => new(new GenericDynamicDbRepository<Model>(new TestDbContextFactory()));
}