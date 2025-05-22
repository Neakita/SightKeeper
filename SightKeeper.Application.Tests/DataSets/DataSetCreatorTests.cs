using FluentAssertions;
using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.Tests.DataSets.Fakes;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
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
		var creator = CreateCreator();
		var data = new FakeNewDataSetData(name, description);
		var dataSet = creator.Create(data);
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldAddDataSetToRepository()
	{
		var creator = CreateCreator(out var repository);
		var data = new FakeNewDataSetData();
		var dataSet = creator.Create(data);
		repository.Received().Add(dataSet);
	}

	[Fact]
	public void ShouldCreateClassifierDataSet()
	{
		var creator = CreateCreator();
		var data = new FakeNewDataSetData(DataSetType.Classifier);
		var dataSet = creator.Create(data);
		dataSet.Should().BeOfType<ClassifierDataSet>();
	}

	[Fact]
	public void ShouldCreateDetectorDataSet()
	{
		var creator = CreateCreator();
		var data = new FakeNewDataSetData(DataSetType.Detector);
		var dataSet = creator.Create(data);
		dataSet.Should().BeOfType<DetectorDataSet>();
	}

	[Fact]
	public void ShouldCreatePoser2DDataSet()
	{
		var creator = CreateCreator();
		var data = new FakeNewDataSetData(DataSetType.Poser2D);
		var dataSet = creator.Create(data);
		dataSet.Should().BeOfType<Poser2DDataSet>();
	}

	[Fact]
	public void ShouldCreatePoser3DDataSet()
	{
		var creator = CreateCreator();
		var data = new FakeNewDataSetData(DataSetType.Poser3D);
		var dataSet = creator.Create(data);
		dataSet.Should().BeOfType<Poser3DDataSet>();
	}

	[Fact]
	public void ShouldNotAddDataSetToRepositoryWhenValidationFails()
	{
		var creator = CreateCreatorWithImpassableValidator(out var repository);
		var data = new FakeNewDataSetData();
		Assert.Throws<ValidationException>(() => creator.Create(data));
		repository.DidNotReceive().Add(Arg.Any<DataSet>());
	}

	[Fact]
	public void ShouldCreateDataSetWithTags()
	{
		const string tagName = "the tag";
		var creator = CreateCreator();
		var data = FakeNewDataSetData.CreateWithTags(tagName);
		var dataSet = creator.Create(data);
		dataSet.TagsLibrary.Tags.Should().Contain(tag => tag.Name == tagName);
	}

	[Fact]
	public void ShouldCreateDataSetWithKeyPointTag()
	{
		const string keyPointTagName = "the key point tag";
		var creator = CreateCreator();
		var data = FakeNewDataSetData.CreateWithKeyPointTags(keyPointTagName);
		var dataSet = creator.Create(data);
		dataSet.Should().BeAssignableTo<PoserDataSet>().Which.TagsLibrary.Tags
			.Should().Contain(tag => tag.KeyPointTags.Any(keyPointTag => keyPointTag.Name == keyPointTagName));
	}

	private static DataSetCreator CreateCreator()
	{
		DataSetCreator creator = new()
		{
			Validator = CreateValidator(),
			Repository = Substitute.For<WriteRepository<DataSet>>()
		};
		return creator;
	}

	private static DataSetCreator CreateCreator(out WriteRepository<DataSet> repository)
	{
		repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = CreateValidator(),
			Repository = repository
		};
		return creator;
	}

	private static IValidator<NewDataSetData> CreateValidator()
	{
		return new InlineValidator<DataSetData>();
	}

	private static DataSetCreator CreateCreatorWithImpassableValidator(out WriteRepository<DataSet> repository)
	{
		repository = Substitute.For<WriteRepository<DataSet>>();
		DataSetCreator creator = new()
		{
			Validator = CreateImpassableValidator(),
			Repository = repository
		};
		return creator;
	}

	private static IValidator<NewDataSetData> CreateImpassableValidator()
	{
		InlineValidator<DataSetData> validator = new();
		validator.RuleFor(data => data.Name).Must(_ => false);
		return validator;
	}
}