using CommunityToolkit.Diagnostics;
using Compunet.YoloV8;
using Compunet.YoloV8.Data;
using Compunet.YoloV8.Metadata;
using SerilogTimings;
using SightKeeper.Application.Prediction;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;
using SixLabors.ImageSharp;
using RectangleF = System.Drawing.RectangleF;

namespace SightKeeper.Services.Prediction;

public sealed class ONNXDetector : Detector
{
    private readonly WeightsDataAccess _weightsDataAccess;


    private async void SetWeights(Weights weights)
    {
        var weightsData = await _weightsDataAccess.LoadWeightsData(weights, WeightsFormat.ONNX);
        _predictor = new YoloV8(new ModelSelector(weightsData.Content), CreateMetadata(weights.Library.DataSet));
        _predictor.Parameters.Confidence = ProbabilityThreshold;
    }
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

    public float ProbabilityThreshold
    {
        get => _predictor?.Parameters.Confidence ?? 0.5f;
        set
        {
            Guard.IsNotNull(_predictor);
            Guard.IsNotNull(value);
            _predictor.Parameters.Confidence = value;
        }
    }

    public float IoU
    {
        get => _probabilityThreshold;
        set
        {
            _probabilityThreshold = value;
            if (_predictor != null)
                _predictor.Parameters.Confidence = value;
        }
    }

    public ONNXDetector(WeightsDataAccess weightsDataAccess)
    {
        _weightsDataAccess = weightsDataAccess;
    }

    public async Task<IReadOnlyCollection<DetectionItem>> Detect(byte[] image, CancellationToken cancellationToken)
    {
        using var operation = Operation.Begin("Detecting on image");
        Guard.IsNotNull(_predictor);
        var detectionResult = await Task.Run(() => _predictor.Detect(new ImageSelector(image)), cancellationToken);
        var result = detectionResult.Boxes.Select(CreateDetectionItem).ToList();
        operation.Complete(nameof(result), result);
        return result;
    }

    private float _probabilityThreshold = 0.5f;
    private YoloV8? _predictor;
    private Weights? _weights;
    private Dictionary<int, ItemClass>? _itemClasses;

    private static YoloV8Metadata CreateMetadata(DataSet dataSet) => new(
        string.Empty, string.Empty, string.Empty,
        YoloV8Task.Detect,
        new Size(dataSet.Resolution),
        dataSet.ItemClasses.Select((itemClass, index) => new YoloV8Class(index, itemClass.Name)).ToList());

    private DetectionItem CreateDetectionItem(IBoundingBox bounding)
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