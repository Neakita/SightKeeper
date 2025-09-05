using FluentValidation;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class NewDataSetDataValidator : AbstractValidator<NewDataSetData>
{
	public NewDataSetDataValidator(ReadRepository<DataSet<Tag, Asset>> dataSetsRepository)
	{
		_dataSetsRepository = dataSetsRepository;
		Include(DataSetDataValidator.Instance);
		RuleFor(data => data.Name)
			.Must((_, dataSetName) => IsNameFree(dataSetName))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadRepository<DataSet<Tag, Asset>> _dataSetsRepository;

	private bool IsNameFree(string name)
	{
		return _dataSetsRepository.Items.All(dataSet => dataSet.Name != name);
	}
}