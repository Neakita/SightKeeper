using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Services.Model;

public sealed class DbModelCreator : ModelCreator
{
    public IObservable<Domain.Model.DataSet> ModelCreated => _modelCreated.AsObservable();
    
    public DbModelCreator(IValidator<ModelData> validator, AppDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public async Task<Domain.Model.DataSet> CreateModel(NewModelDataDTO data, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(data, cancellationToken);
        var model = data.ModelType switch
        {
            ModelType.Detector => new DetectorDataSet(data.Name, data.Resolution),
            ModelType.Classifier => throw new NotSupportedException("Classifier model creation not implemented yet"),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<Domain.Model.DataSet>(nameof(data.ModelType))
        };
        _dbContext.Models.Add(model);
        model.Description = data.Description;
        foreach (var itemClass in data.ItemClasses)
            model.CreateItemClass(itemClass);
        model.Game = data.Game;
        await _dbContext.SaveChangesAsync(cancellationToken);
        _modelCreated.OnNext(model);
        return model;
    }
    
    private readonly IValidator<ModelData> _validator;
    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.DataSet> _modelCreated = new();
}