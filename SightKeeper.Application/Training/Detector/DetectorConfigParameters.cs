using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Training;

public sealed class DetectorConfigParameters
{
    public ushort Batch { get; set; }
    public ushort Subdivisions { get; set; }
    public ushort Width { get; set; }
    public ushort Height { get; set; }
    public ushort ClassesCount { get; set; }
    public int MaxBatches => Math.Max(6000, ClassesCount * 2);
    public ushort Steps80 => (ushort) (MaxBatches / 100 * 80);
    public ushort Steps90 => (ushort) (MaxBatches / 100 * 90);
    public ushort Filters => (ushort) ((ClassesCount + 5) * 3);
    public ushort GaussianFilters => (ushort) ((ClassesCount + 9) * 3);

    public DetectorConfigParameters(ushort width, ushort height, ushort classesCount)
        : this(1, 1, width, height, classesCount)
    {
    }
    
    public DetectorConfigParameters(ushort batch, ushort subdivisions, ushort width, ushort height, ushort classesCount)
    {
        Batch = batch;
        Subdivisions = subdivisions;
        Width = width;
        Height = height;
        ClassesCount = classesCount;
    }

    public string Deploy(ModelConfig config)
    {
        Dictionary<string, string> replaceables = new()
        {
            {DetectorConfig.BatchPlaceholder, Batch.ToString()},
            {DetectorConfig.SubdivisionsPlaceholder, Subdivisions.ToString()},
            {DetectorConfig.WidthPlaceholder, Width.ToString()},
            {DetectorConfig.HeightPlaceholder, Height.ToString()},
            {DetectorConfig.MaxBatchesPlaceholder, MaxBatches.ToString()},
            {DetectorConfig.Steps80Placeholder, Steps80.ToString()},
            {DetectorConfig.Steps90Placeholder, Steps90.ToString()},
            {DetectorConfig.ClassesCountPlaceholder, ClassesCount.ToString()},
            {DetectorConfig.FiltersPlaceholder, Filters.ToString()},
            {DetectorConfig.GaussianFiltersPlaceholder, GaussianFilters.ToString()}
        };
        string result = config.Content;
        foreach ((string placeholder, string value) in replaceables)
            while (result.Contains(placeholder))
                result = result.Replace(placeholder, value);
        return result;
    }
}