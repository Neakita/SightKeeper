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
	public void ShouldSetProvidedNameAndDescription()
	{
		const string name = "the name";
		const string description = "the description";
		var dataSet = CreateDataSetViaCreator(name, description);
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldAddDataSetToRepository()
	{
		var dataSet = CreateDataSetViaCreator(out var repository);
		repository.Received().Add(dataSet);
	}

	[Fact]
	public void ShouldCreateClassifierDataSet()
	{
		var dataSet = CreateDataSetViaCreator(DataSetType.Classifier);
		dataSet.Should().BeOfType<ClassifierDataSet>();
	}

	[Fact]
	public void ShouldCreateDetectorDataSet()
	{
		var dataSet = CreateDataSetViaCreator(DataSetType.Detector);
		dataSet.Should().BeOfType<DetectorDataSet>();
	}

	[Fact]
	public void ShouldCreatePoser2DDataSet()
	{
		var dataSet = CreateDataSetViaCreator(DataSetType.Poser2D);
		dataSet.Should().BeOfType<Poser2DDataSet>();
	}

	[Fact]
	public void ShouldCreatePoser3DDataSet()
	{
		var dataSet = CreateDataSetViaCreator(DataSetType.Poser3D);
		dataSet.Should().BeOfType<Poser3DDataSet>();
	}

	[Fact]
	public void ShouldNotAddDataSetToRepositoryWhenValidationFails()
	{
		AttemptToCreateDataSetWithValidationExceptionProvocation(out var repository);
		repository.DidNotReceive().Add(Arg.Any<DataSet>());
	}

	private static DataSet CreateDataSetViaCreator(string name, string description)
	{
		DataSetCreator creator = new()
		{
			Validator = CreateValidator(),
			Repository = Substitute.For<WriteRepository<DataSet>>()
		};
		var data = Utilities.CreateNewDataSetData(name, description);
		return creator.Create(data);
	}

	private static DataSet CreateDataSetViaCreator(out WriteRepository<DataSet> repository)
	{
		repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = CreateValidator(),
			Repository = repository
		};
		var data = Utilities.CreateNewDataSetData();
		return creator.Create(data);
	}

	private static DataSet CreateDataSetViaCreator(DataSetType type)
	{
		DataSetCreator creator = new()
		{
			Validator = CreateValidator(),
			Repository = Substitute.For<WriteRepository<DataSet>>()
		};
		var data = Utilities.CreateNewDataSetData(type: type);
		var dataSet = creator.Create(data);
		return dataSet;
	}

	private static void AttemptToCreateDataSetWithValidationExceptionProvocation(out WriteRepository<DataSet> repository)
	{
		repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = CreateImpassableValidator(),
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