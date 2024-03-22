using FlakeId;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbWeightsDataAccess : WeightsDataAccess
{
    public DbWeightsDataAccess(AppDbContext dbContext)
    {
	    _dbContext = dbContext;
	    _dbContext.SavedChanges += OnDbContextSavedChanges;
    }

    public override WeightsData LoadWeightsONNXData(Weights weights)
    {
	    return LoadWeightsData(weights, DbWeightsData.DataFormat.ONNX);
    }
    public override WeightsData LoadWeightsPTData(Weights weights)
    {
	    return LoadWeightsData(weights, DbWeightsData.DataFormat.PT);
    }

    protected override void SaveWeightsData(Weights weights, WeightsData onnxData, WeightsData ptData)
    {
	    DbWeightsData onnxWeightsData = new(onnxData, weights, DbWeightsData.DataFormat.ONNX);
	    _dbContext.Add(onnxWeightsData);
	    _unsavedWeightsData.Add((weights, DbWeightsData.DataFormat.ONNX), onnxData);
	    DbWeightsData ptWeightsData = new(ptData, weights, DbWeightsData.DataFormat.PT);
	    _dbContext.Add(ptWeightsData);
	    _unsavedWeightsData.Add((weights, DbWeightsData.DataFormat.PT), ptData);
    }
    protected override void RemoveWeightsData(Weights weights)
    {
	    _unsavedWeightsData.Remove((weights, DbWeightsData.DataFormat.ONNX));
	    _unsavedWeightsData.Remove((weights, DbWeightsData.DataFormat.PT));
    }

    private readonly AppDbContext _dbContext;
    private readonly Dictionary<(Weights, DbWeightsData.DataFormat), WeightsData> _unsavedWeightsData = new();

    private WeightsData LoadWeightsData(Weights weights, DbWeightsData.DataFormat format)
    {
	    if (_unsavedWeightsData.TryGetValue((weights, format), out var unsavedWeightsData))
		    return unsavedWeightsData;
	    var weightsEntry = _dbContext.Entry(weights);
	    var weightsId = weightsEntry.Property<Id>("Id").CurrentValue;
	    return _dbContext.Set<DbWeightsData>()
		    .AsNoTracking()
		    .Where(weightsData => EF.Property<Id>(weightsData.Weights, "Id") == weightsId && weightsData.Format == format)
		    .Select(weightsData => weightsData.Data)
		    .Single();
    }
    private void OnDbContextSavedChanges(object? sender, SavedChangesEventArgs e)
    {
	    _unsavedWeightsData.Clear();
    }
}