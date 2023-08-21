using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Application.DataSet;
using SightKeeper.Application.DataSet.Creating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Services.DataSet;

public sealed class DbDataSetCreator : DataSetCreator
{
    public IObservable<Domain.Model.DataSet> DataSetCreated => _dataSetCreated.AsObservable();
    
    public DbDataSetCreator(IValidator<DataSetInfo> validator, AppDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public async Task<Domain.Model.DataSet> CreateDataSet(NewDataSetInfoDTO newDataSetInfo, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(newDataSetInfo, cancellationToken);
        var dataSet = newDataSetInfo.ModelType switch
        {
            ModelType.Detector => new DataSet<DetectorAsset>(newDataSetInfo.Name, newDataSetInfo.Resolution),
            ModelType.Classifier => throw new NotSupportedException("Classifier data set creation not implemented yet"),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<Domain.Model.DataSet>(nameof(newDataSetInfo.ModelType))
        };
        dataSet.Description = newDataSetInfo.Description;
        foreach (var itemClass in newDataSetInfo.ItemClasses)
            dataSet.CreateItemClass(itemClass);
        dataSet.Game = newDataSetInfo.Game;
        _dbContext.DataSets.Add(dataSet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dataSetCreated.OnNext(dataSet);
        return dataSet;
    }
    
    private readonly IValidator<NewDataSetInfo> _validator;
    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.DataSet> _dataSetCreated = new();
}

