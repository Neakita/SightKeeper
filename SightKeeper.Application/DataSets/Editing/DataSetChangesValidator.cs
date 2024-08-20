﻿using FluentValidation;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class DataSetChangesValidator : AbstractValidator<DataSetChanges>
{
    public DataSetChangesValidator(IValidator<GeneralDataSetInfo> dataSetInfoValidator, ReadDataAccess<DataSet> dataSetsDataAccess)
    {
        _dataSetsDataAccess = dataSetsDataAccess;
        Include(dataSetInfoValidator);
        RuleFor(data => data.Name)
            .NotEmpty()
            .Must((changes, _) => IsNameFree(changes)).WithMessage("Name must be unique");
    }

    private bool IsNameFree(DataSetChanges changes)
    {
	    return _dataSetsDataAccess.Items.All(dataSet => dataSet == changes.DataSet || dataSet.Name != changes.Name);
    }

    private readonly ReadDataAccess<DataSet> _dataSetsDataAccess;
}