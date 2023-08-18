using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Services.DataSet;

public sealed class DbDataSetCreator : DataSetCreator
{
    public IObservable<Domain.Model.DataSet> ModelCreated => _modelCreated.AsObservable();
    
    public DbDataSetCreator(IValidator<DataSetData> validator, AppDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public async Task<Domain.Model.DataSet> CreateDataSet(NewDataSetDataSetDataSetDataDTO dataSetDataSetDataSetData, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(dataSetDataSetDataSetData, cancellationToken);
        var model = dataSetDataSetDataSetData.ModelType switch
        {
            ModelType.Detector => new DetectorDataSet(dataSetDataSetDataSetData.Name, dataSetDataSetDataSetData.Resolution),
            ModelType.Classifier => throw new NotSupportedException("Classifier model creation not implemented yet"),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<Domain.Model.DataSet>(nameof(dataSetDataSetDataSetData.ModelType))
        };
        _dbContext.Models.Add(model);
        model.Description = dataSetDataSetDataSetData.Description;
        foreach (var itemClass in dataSetDataSetDataSetData.ItemClasses)
            model.CreateItemClass(itemClass);
        model.Game = dataSetDataSetDataSetData.Game;
        await _dbContext.SaveChangesAsync(cancellationToken);
        _modelCreated.OnNext(model);
        return model;
    }
    
    private readonly IValidator<DataSetData> _validator;
    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.DataSet> _modelCreated = new();
}