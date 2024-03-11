using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using Compunet.YoloV8;
using Compunet.YoloV8.Data;
using Compunet.YoloV8.Metadata;
using SerilogTimings;
using SightKeeper.Application.Prediction;
using SightKeeper.Domain.Model.DataSets;
using SixLabors.ImageSharp;
using RectangleF = System.Drawing.RectangleF;
using Size = SixLabors.ImageSharp.Size;

namespace SightKeeper.Services.Prediction;

public sealed class ONNXDetector(WeightsDataAccess weightsDataAccess) : Detector
{
    public Weights? Weights
    {
        get => _weights;
        set
        {
            _weights = value;
            _predictor?.Dispose();
            _predictor = null;
            _itemClasses = null;
            if (value == null)
                return;
            SetWeights(value);
            _itemClasses = value.Library.DataSet.ItemClasses
                .Select((itemClass, itemClassIndex) => (itemClass, itemClassIndex))
                .ToDictionary(t => t.itemClassIndex, t => t.itemClass);
        }
    }
    
    private void SetWeights(Weights weights)
    {
        var weightsData = weightsDataAccess.LoadWeightsONNXData(weights);
        _predictor = new YoloV8(new ModelSelector(weightsData.Content), CreateMetadata(weights.Library.DataSet));
        _predictor.Parameters.Confidence = ProbabilityThreshold;
        _predictor.Parameters.IoU = IoU;
    }

    public float ProbabilityThreshold
    {
        get => _probabilityThreshold;
        set
        {
            _probabilityThreshold = value;
            if (_predictor != null)
                _predictor.Parameters.Confidence = value;
        }
    }

    public float IoU
    {
        get => _iou;
        set
        {
            _iou = value;
            if (_predictor != null)
                _predictor.Parameters.IoU = value;
        }
    }

    public ImmutableList<DetectionItem> Detect(byte[] image)
    {
        using var operation = Operation.Begin("Detecting on image");
        Guard.IsNotNull(_predictor);
        var detectionResult = _predictor.Detect(new ImageSelector(image));
        var result = detectionResult.Boxes.Select(CreateDetectionItem).ToImmutableList();
        operation.Complete("count", result.Count);
        return result;
    }

    public async Task<ImmutableList<DetectionItem>> DetectAsync(byte[] image, CancellationToken cancellationToken = default)
    {
        using var operation = Operation.Begin("Detecting on image");
        Guard.IsNotNull(_predictor);
        var detectionResult = await Task.Run(() => _predictor.Detect(new ImageSelector(image)), cancellationToken);
        var result = detectionResult.Boxes.Select(CreateDetectionItem).ToImmutableList();
        operation.Complete("count", result.Count);
        return result;
    }

    private float _probabilityThreshold = YoloV8Parameters.Default.Confidence;
    private float _iou = YoloV8Parameters.Default.IoU;
    private YoloV8? _predictor;
    private Weights? _weights;
    private Dictionary<int, ItemClass>? _itemClasses;

    private static YoloV8Metadata CreateMetadata(DataSet dataSet) => new(
        string.Empty, string.Empty, string.Empty,
        YoloV8Task.Detect,
        1,
        new Size(dataSet.Resolution),
        dataSet.ItemClasses.Select((itemClass, index) => new YoloV8Class(index, itemClass.Name)).ToList());

    private DetectionItem CreateDetectionItem(BoundingBox bounding)
    {
        Guard.IsNotNull(_itemClasses);
        Guard.IsNotNull(_weights);
        return new DetectionItem(_itemClasses[bounding.Class.Id], CreateBounding(bounding.Bounds, _weights.Library.DataSet.Resolution), bounding.Confidence);
    }

    private static RectangleF CreateBounding(Rectangle rectangle, ushort resolution) => new(
        (float)rectangle.X / resolution,
        (float)rectangle.Y / resolution,
        (float)rectangle.Width / resolution,
        (float)rectangle.Height / resolution);
}