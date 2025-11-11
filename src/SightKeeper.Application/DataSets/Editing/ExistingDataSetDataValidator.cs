using FluentValidation;
using SightKeeper.Application.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

internal sealed class ExistingDataSetDataValidator : AbstractValidator<ExistingDataSetData>
{
	public ExistingDataSetDataValidator(ReadRepository<DataSet<Tag, Asset>> dataSetsRepository)
	{
		_dataSetsRepository = dataSetsRepository;
		Include(DataSetDataValidator.Instance);
		RuleFor(data => data.Name)
			.Must((data, name) => IsNameFree(data.DataSet, name))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadRepository<DataSet<Tag, Asset>> _dataSetsRepository;

	private bool IsNameFree(DataSet<Tag, Asset> subject, string name)
	{
		return _dataSetsRepository.Items.All(dataSet => dataSet == subject || dataSet.Name != name);
	}
}