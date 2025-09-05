using FluentValidation;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class ExistingDataSetDataValidator : AbstractValidator<ExistingDataSetData>
{
	public ExistingDataSetDataValidator(ReadRepository<DataSet<Asset>> dataSetsRepository)
	{
		_dataSetsRepository = dataSetsRepository;
		Include(DataSetDataValidator.Instance);
		RuleFor(data => data.Name)
			.Must((data, name) => IsNameFree(data.DataSet, name))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadRepository<DataSet<Asset>> _dataSetsRepository;

	private bool IsNameFree(DataSet<Asset> subject, string name)
	{
		return _dataSetsRepository.Items.All(dataSet => dataSet == subject || dataSet.Name != name);
	}
}