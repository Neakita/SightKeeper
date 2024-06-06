using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using Compunet.YoloV8;
using Compunet.YoloV8.Data;
using Compunet.YoloV8.Metadata;
using SerilogTimings;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Services;
using SixLabors.ImageSharp;
using RectangleF = System.Drawing.RectangleF;
using Size = SixLabors.ImageSharp.Size;

namespace SightKeeper.Application.Prediction;

public sealed class ONNXDetector : Detector
{
    public Weights? Weights
    {
        get => _weights;
        set
        {
            _weights = value;
            _predictor?.Dispose();
            _predictor = null;
            _tags = null;
            if (value == null)
                return;
            SetWeights(value);
            _tags = _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(value)).Tags
                .Select((tag, tagIndex) => (tag, tagIndex))
                .ToDictionary(t => t.tagIndex, t => t.tag);
        }
    }
    
    private void SetWeights(Weights weights)
    {
        var weightsData = _weightsDataAccess.LoadWeightsONNXData(weights);
        YoloV8Builder builder = new();
        builder.UseOnnxModel(new BinarySelector(weightsData.Content));
        builder.WithMetadata(CreateMetadata(_objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(weights))));
        builder.WithConfiguration(configuration =>
        {
	        configuration.Confidence = ProbabilityThreshold;
	        configuration.IoU = IoU;
        });
        _predictor = builder.Build();
    }

    public float ProbabilityThreshold
    {
        get => _probabilityThreshold;
        set
        {
            _probabilityThreshold = value;
            if (_predictor != null)
                _predictor.Configuration.Confidence = value;
        }
    }

    public float IoU
    {
        get => _iou;
        set
        {
            _iou = value;
            if (_predictor != null)
                _predictor.Configuration.IoU = value;
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

    private readonly WeightsDataAccess _weightsDataAccess;
    private readonly ObjectsLookupper _objectsLookupper;
    private float _probabilityThreshold = YoloV8Configuration.Default.Confidence;
    private float _iou = YoloV8Configuration.Default.IoU;
    private YoloV8Predictor? _predictor;
    private Weights? _weights;
    private Dictionary<int, Tag>? _tags;

    public ONNXDetector(WeightsDataAccess weightsDataAccess, ObjectsLookupper objectsLookupper)
    {
	    _weightsDataAccess = weightsDataAccess;
	    _objectsLookupper = objectsLookupper;
    }

    private static YoloV8Metadata CreateMetadata(DetectorDataSet dataSet) => new(
        string.Empty, string.Empty, string.Empty,
        YoloV8Task.Detect,
        1,
        new Size(dataSet.Resolution),
        dataSet.Tags.Select((tag, index) => new YoloV8Class(index, tag.Name)).ToList());

    private DetectionItem CreateDetectionItem(BoundingBox bounding)
    {
        Guard.IsNotNull(_tags);
        Guard.IsNotNull(_weights);
        return new DetectionItem(_tags[bounding.Class.Id], CreateBounding(bounding.Bounds, _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(_weights)).Resolution), bounding.Confidence);
    }

    private static RectangleF CreateBounding(Rectangle rectangle, ushort resolution) => new(
        (float)rectangle.X / resolution,
        (float)rectangle.Y / resolution,
        (float)rectangle.Width / resolution,
        (float)rectangle.Height / resolution);
}