using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;

namespace SightKeeper.Data.Services.DataSet;

public sealed class DbDataSetCreator : DataSetCreator
{
    public IObservable<Domain.Model.DataSets.DataSet> DataSetCreated => _dataSetCreated.AsObservable();
    
    public DbDataSetCreator(IValidator<DataSetInfo> validator, AppDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public async Task<Domain.Model.DataSets.DataSet> CreateDataSet(NewDataSetInfoDTO newDataSetInfo, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(newDataSetInfo, cancellationToken);
        var dataSet = new Domain.Model.DataSets.DataSet(newDataSetInfo.Name, newDataSetInfo.Resolution);
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
    private readonly Subject<Domain.Model.DataSets.DataSet> _dataSetCreated = new();
}

