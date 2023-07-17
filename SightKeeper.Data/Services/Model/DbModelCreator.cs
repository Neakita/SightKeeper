using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Services.Model;

public sealed class DbModelCreator : ModelCreator
{
    public DbModelCreator(IValidator<ModelData> validator, AppDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public async Task<Domain.Model.Model> CreateModel(NewModelDataDTO data, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(data, cancellationToken);
        var model = data.ModelType switch
        {
            ModelType.Detector => new DetectorModel(data.Name, data.Resolution),
            ModelType.Classifier => throw new NotImplementedException("Classifier model creation not implemented yet"),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<Domain.Model.Model>(nameof(data.ModelType))
        };
        _dbContext.Models.Add(model);
        model.Description = data.Description;
        foreach (var itemClass in data.ItemClasses)
            model.CreateItemClass(itemClass);
        model.Game = data.Game;
        model.Config = data.Config;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return model;
    }
    
    private readonly IValidator<ModelData> _validator;
    private readonly AppDbContext _dbContext;
}