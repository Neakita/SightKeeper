using FluentValidation;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class NewDataSetDataValidator : AbstractValidator<DataSetData>
{
	public NewDataSetDataValidator(
		IValidator<DataSetData> dataSetDataValidator,
		ReadRepository<DataSet> dataSetsRepository)
	{
		_dataSetsRepository = dataSetsRepository;
		Include(dataSetDataValidator);
		RuleFor(data => data.Name)
			.Must((_, dataSetName) => IsNameFree(dataSetName))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadRepository<DataSet> _dataSetsRepository;

	private bool IsNameFree(string name)
	{
		return _dataSetsRepository.Items.All(dataSet => dataSet.Name != name);
	}
}