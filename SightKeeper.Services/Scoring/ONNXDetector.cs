using CommunityToolkit.Diagnostics;
using Compunet.YoloV8;
using Compunet.YoloV8.Data;
using Compunet.YoloV8.Metadata;
using SerilogTimings;
using SightKeeper.Application.Scoring;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SixLabors.ImageSharp;
using RectangleF = System.Drawing.RectangleF;

namespace SightKeeper.Services.Scoring;

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
            _itemClasses = null;
            if (value == null)
                return;
            _predictor = new YoloV8(new ModelSelector(value.Data), CreateMetadata(value.Library.DataSet));
            _itemClasses = value.Library.DataSet.ItemClasses
                .Select((itemClass, itemClassIndex) => (itemClass, itemClassIndex))
                .ToDictionary(t => t.itemClassIndex, t => t.itemClass);
        }
    }

    public float? ProbabilityThreshold
    {
        get => _predictor?.Parameters.Confidence ?? 0.5f;
        set
        {
            Guard.IsNotNull(_predictor);
            Guard.IsNotNull(value);
            _predictor.Parameters.Confidence = value.Value;
        }
    }

    public float? IoU
    {
        get => _predictor?.Parameters.IoU;
        set
        {
            Guard.IsNotNull(_predictor);
            Guard.IsNotNull(value);
            _predictor.Parameters.IoU = value.Value;
        }
    }

    public IReadOnlyCollection<DetectionItem> Detect(byte[] image)
    {
        Guard.IsNotNull(_predictor);
        var result = _predictor.Detect(new ImageSelector(image));
        return result.Boxes.Select(CreateDetectionItem).ToList();
    }

    public async Task<IReadOnlyCollection<DetectionItem>> Detect(byte[] image, CancellationToken cancellationToken)
    {
        using Operation operation = Operation.Begin("Detecting on image");
        Guard.IsNotNull(_predictor);
        var detectionResult = await Task.Run(() => _predictor.Detect(new ImageSelector(image)), cancellationToken);
        var result = detectionResult.Boxes.Select(CreateDetectionItem).ToList();
        operation.Complete(nameof(result), result);
        return result;
    }

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