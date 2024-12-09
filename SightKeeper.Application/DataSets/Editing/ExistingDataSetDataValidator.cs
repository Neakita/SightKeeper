using FluentValidation;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class ExistingDataSetDataValidator : AbstractValidator<DataSetData>
{
	public ExistingDataSetDataValidator(
		DataSet dataSet,
		IValidator<DataSetData> dataSetDataValidator,
		ReadDataAccess<DataSet> dataSetsDataAccess)
	{
		_dataSet = dataSet;
		_dataSetsDataAccess = dataSetsDataAccess;
		Include(dataSetDataValidator);
		RuleFor(data => data.Name)
			.Must((_, dataSetName) => IsNameFree(dataSetName))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly DataSet _dataSet;
	private readonly ReadDataAccess<DataSet> _dataSetsDataAccess;

	private bool IsNameFree(string name)
	{
		return _dataSetsDataAccess.Items.All(dataSet => dataSet == _dataSet || dataSet.Name != name);
	}
}