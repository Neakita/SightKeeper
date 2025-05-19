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
		DataSetCreator creator = new(validator, repository);
		const string name = "the name";
		const string description = "the description";
		var data = Utilities.CreateDataSetData(name, description);
		var dataSet = creator.Create(data, DataSetType.Classifier);
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
		repository.Received().Add(dataSet);
	}

	[Fact]
	public void ShouldCreateClassifierDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new(validator, repository);
		var data = Utilities.CreateDataSetData();
		var dataSet = creator.Create(data, DataSetType.Classifier);
		dataSet.Should().BeOfType<ClassifierDataSet>();
	}

	[Fact]
	public void ShouldCreateDetectorDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new(validator, repository);
		var data = Utilities.CreateDataSetData();
		var dataSet = creator.Create(data, DataSetType.Detector);
		dataSet.Should().BeOfType<DetectorDataSet>();
	}

	[Fact]
	public void ShouldCreatePoser2DDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new(validator, repository);
		var data = Utilities.CreateDataSetData();
		var dataSet = creator.Create(data, DataSetType.Poser2D);
		dataSet.Should().BeOfType<Poser2DDataSet>();
	}

	[Fact]
	public void ShouldCreatePoser3DDataSet()
	{
		var validator = CreateValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new(validator, repository);
		var data = Utilities.CreateDataSetData();
		var dataSet = creator.Create(data, DataSetType.Poser3D);
		dataSet.Should().BeOfType<Poser3DDataSet>();
	}

	[Fact]
	public void ShouldNotCreateDataSetWhenValidationFails()
	{
		var validator = CreateImpassableValidator();
		var repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new(validator, repository);
		var data = Utilities.CreateDataSetData();
		Assert.Throws<ValidationException>(() => creator.Create(data, DataSetType.Classifier));
	}

	private static IValidator<DataSetData> CreateValidator()
	{
		return new InlineValidator<DataSetData>();
	}

	private static IValidator<DataSetData> CreateImpassableValidator()
	{
		InlineValidator<DataSetData> validator = new();
		validator.RuleFor(data => data.Name).Must(_ => false);
		return validator;
	}
}