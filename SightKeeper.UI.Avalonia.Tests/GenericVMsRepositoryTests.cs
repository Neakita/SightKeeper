using FluentAssertions;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Services;
using SightKeeper.Tests.Common;
using SightKeeper.UI.Avalonia.ViewModels.Components;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.Tests;

public sealed class GenericVMsRepositoryTests
{
	[Fact]
	public void ShouldGetSameInstanceAfterAdd()
	{
		AppBootstrapper.Setup();
		GenericVMsRepository<ModelVM, Model> repository = Repository;
		DetectorModelVMImplementation testModel = new(new DetectorModel("Test model"));
		repository.Add(testModel);
		repository.Items.Single().Should().BeSameAs(testModel);
	}

	[Fact]
	public void ShouldAddInstanceFromInnerRepository()
	{
		AppBootstrapper.Setup();
		var innerRepository = new GenericDynamicDbRepository<Model>(new TestDbContextFactory());
		GenericVMsRepository<ModelVM, Model> repository = new(innerRepository);
		DetectorModel testModel = new("Test model");
		innerRepository.Add(testModel);
		repository.Items.Should().ContainSingle(modelVM => modelVM.Item == testModel);
	}

	[Fact]
	public void ShouldDeleteModelVMFromModelRepository()
	{
		AppBootstrapper.Setup();
		var innerRepository = new GenericDynamicDbRepository<Model>(new TestDbContextFactory());
		GenericVMsRepository<ModelVM, Model> repository = new(innerRepository);
		DetectorModel testModel = new("Test model");
		DetectorModelVMImplementation testModelVMImplementation = new(testModel);
		repository.Add(testModelVMImplementation);
		repository.Items.Single().Should().BeSameAs(testModelVMImplementation);
		
		innerRepository.Remove(testModel);
		repository.Items.Should().BeEmpty();
	}

	[Fact]
	public void ShouldDeleteModelFromModelVMRepository()
	{
		AppBootstrapper.Setup();
		var innerRepository = new GenericDynamicDbRepository<Model>(new TestDbContextFactory());
		GenericVMsRepository<ModelVM, Model> repository = new(innerRepository);
		DetectorModel testModel = new("Test model");
		innerRepository.Add(testModel);
		repository.Items.Should().ContainSingle(modelVM => modelVM.Item == testModel);
		repository.Remove(repository.Items.Single());
		innerRepository.Items.Should().BeEmpty();
	}
	
	private static GenericVMsRepository<ModelVM, Model> Repository => new(new GenericDynamicDbRepository<Model>(new TestDbContextFactory()));
}
