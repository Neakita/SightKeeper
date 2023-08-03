using Avalonia;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class BoundingBoxViewModel : ViewModel
{
    private static readonly string[] Properties =
    {
        nameof(X1),
        nameof(X2),
        nameof(Y1),
        nameof(Y2),
        nameof(Width),
        nameof(Height),
        nameof(XCenter),
        nameof(YCenter)
    };

    public BoundingBox Bounding { get; set; }
    public double X1 { get; private set; }
    public double Y1 { get; private set; }
    public double X2 { get; private set; }
    public double Y2 { get; private set; }
    public double Width => X2 - X1;
    public double Height => Y2 - Y1;
    public double XCenter => X1 + Width / 2;
    public double YCenter => Y1 + Height / 2;

    public BoundingBoxViewModel(Point position) : this()
    {
        X1 = position.X;
        X2 = position.X;
        Y1 = position.Y;
        Y2 = position.Y;
    }

    public BoundingBoxViewModel(BoundingBox bounding)
    {
        Bounding = bounding;
        X1 = bounding.X1;
        Y1 = bounding.Y1;
        X2 = bounding.X2;
        Y2 = bounding.Y2;
    }

    public BoundingBoxViewModel()
    {
        Bounding = new BoundingBox();
    }

    public void SetFromTwoPositions(Point position1, Point position2) =>
        SetFromTwoPositions(position1.X, position1.Y, position2.X, position2.Y);

    public void SetFromTwoPositions(double x1, double y1, double x2, double y2)
    {
        MinMax(x1, x2, out var xMin, out var xMax); // 🎄
        MinMax(y1, y2, out var yMin, out var yMax);
        OnPropertiesChanging(Properties);
        X1 = xMin;
        X2 = xMax;
        Y1 = yMin;
        Y2 = yMax;
        OnPropertiesChanged(Properties);
    }

    public void Synchronize()
    {
        Guard.IsNotNull(Bounding);
        Bounding.SetFromTwoPositions(X1, Y1, X2, Y2);
    }

    private static void MinMax(double value1, double value2, out double min, out double max)
    {
        if (value1 < value2)
        {
            min = value1;
            max = value2;
        }
        else
        {
            min = value2;
            max = value1;
        }
    }
}