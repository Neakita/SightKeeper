using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.DataSet;
using SightKeeper.Application.DataSet.Creating;

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
        var dataSet = new Domain.Model.DataSet(newDataSetInfo.Name, newDataSetInfo.Resolution);
        dataSet.Description = newDataSetInfo.Description;
        foreach (var itemClass in newDataSetInfo.ItemClasses)
            dataSet.CreateItemClass(itemClass.Name, itemClass.Color);
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

