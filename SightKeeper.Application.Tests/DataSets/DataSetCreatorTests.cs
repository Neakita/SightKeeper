using FluentAssertions;
using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class DataSetCreatorTests
{
	[Fact]
	public void ShouldCreateDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = validator,
			Repository = repository
		};
		const string name = "the name";
		const string description = "the description";
		var data = Utilities.CreateNewDataSetData(name, description);
		var dataSet = creator.Create(data);
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
		repository.Received().Add(dataSet);
	}

	[Fact]
	public void ShouldCreateClassifierDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = validator,
			Repository = repository
		};
		var data = Utilities.CreateNewDataSetData(type: DataSetType.Classifier);
		var dataSet = creator.Create(data);
		dataSet.Should().BeOfType<ClassifierDataSet>();
	}

	[Fact]
	public void ShouldCreateDetectorDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = validator,
			Repository = repository
		};
		var data = Utilities.CreateNewDataSetData(type: DataSetType.Detector);
		var dataSet = creator.Create(data);
		dataSet.Should().BeOfType<DetectorDataSet>();
	}

	[Fact]
	public void ShouldCreatePoser2DDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = validator,
			Repository = repository
		};
		var data = Utilities.CreateNewDataSetData(type: DataSetType.Poser2D);
		var dataSet = creator.Create(data);
		dataSet.Should().BeOfType<Poser2DDataSet>();
	}

	[Fact]
	public void ShouldCreatePoser3DDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = validator,
			Repository = repository
		};
		var data = Utilities.CreateNewDataSetData(type: DataSetType.Poser3D);
		var dataSet = creator.Create(data);
		dataSet.Should().BeOfType<Poser3DDataSet>();
	}

	[Fact]
	public void ShouldNotCreateDataSetWhenValidationFails()
	{
		var validator = CreateImpassableValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = validator,
			Repository = repository
		};
		var data = Utilities.CreateNewDataSetData();
		Assert.Throws<ValidationException>(() => creator.Create(data));
	}

	private static IValidator<NewDataSetData> CreateValidator()
	{
		return new InlineValidator<DataSetData>();
	}

	private static IValidator<NewDataSetData> CreateImpassableValidator()
	{
		InlineValidator<DataSetData> validator = new();
		validator.RuleFor(data => data.Name).Must(_ => false);
		return validator;
	}
}