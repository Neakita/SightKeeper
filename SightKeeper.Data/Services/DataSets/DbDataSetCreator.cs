using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Services.DataSets;

public sealed class DbDataSetCreator : DataSetCreator
{
    public IObservable<DataSet> DataSetCreated => _dataSetCreated.AsObservable();
    
    public DbDataSetCreator(IValidator<DataSetInfo> validator, AppDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public DataSet CreateDataSet(NewDataSetInfoDTO newDataSetInfo)
    {
        _validator.ValidateAndThrow(newDataSetInfo);
        DataSet dataSet = new(newDataSetInfo.Name, newDataSetInfo.Resolution);
        dataSet.Description = newDataSetInfo.Description;
        foreach (var itemClass in newDataSetInfo.ItemClasses)
            dataSet.CreateItemClass(itemClass.Name, itemClass.Color);
        dataSet.Game = newDataSetInfo.Game;
        _dbContext.DataSets.Add(dataSet);
        _dbContext.SaveChanges();
        _dataSetCreated.OnNext(dataSet);
        return dataSet;
    }
    
    private readonly IValidator<NewDataSetInfo> _validator;
    private readonly AppDbContext _dbContext;
    private readonly Subject<DataSet> _dataSetCreated = new();
}

