using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Services.Model;

public sealed class DbDataSetCreator : DataSetCreator
{
    public IObservable<Domain.Model.DataSet> ModelCreated => _modelCreated.AsObservable();
    
    public DbDataSetCreator(IValidator<ModelData> validator, AppDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public async Task<Domain.Model.DataSet> CreateDataSet(NewDataSetDataDTO dataSetData, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(dataSetData, cancellationToken);
        var model = dataSetData.ModelType switch
        {
            ModelType.Detector => new DetectorDataSet(dataSetData.Name, dataSetData.Resolution),
            ModelType.Classifier => throw new NotSupportedException("Classifier model creation not implemented yet"),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<Domain.Model.DataSet>(nameof(dataSetData.ModelType))
        };
        _dbContext.Models.Add(model);
        model.Description = dataSetData.Description;
        foreach (var itemClass in dataSetData.ItemClasses)
            model.CreateItemClass(itemClass);
        model.Game = dataSetData.Game;
        await _dbContext.SaveChangesAsync(cancellationToken);
        _modelCreated.OnNext(model);
        return model;
    }
    
    private readonly IValidator<ModelData> _validator;
    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.DataSet> _modelCreated = new();
}