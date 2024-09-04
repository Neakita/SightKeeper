using FluentValidation;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class NewDataSetDataValidator : AbstractValidator<DataSetData>
{
	public NewDataSetDataValidator(
		IValidator<DataSetData> dataSetDataValidator,
		ReadDataAccess<DataSet> dataSetsDataAccess)
	{
		_dataSetsDataAccess = dataSetsDataAccess;
		Include(dataSetDataValidator);
		RuleFor(data => data.Name)
			.Must((_, dataSetName) => IsNameFree(dataSetName))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadDataAccess<DataSet> _dataSetsDataAccess;

	private bool IsNameFree(string name)
	{
		return _dataSetsDataAccess.Items.All(dataSet => dataSet.Name != name);
	}
}