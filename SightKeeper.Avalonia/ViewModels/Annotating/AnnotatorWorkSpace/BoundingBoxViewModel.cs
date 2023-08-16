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

    public double X1
    {
        get => _x1;
        set
        {
            Guard.IsLessThanOrEqualTo(value, X2);
            if (!SetProperty(ref _x1, value))
                return;
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(XCenter));
        }
    }

    public double Y1
    {
        get => _y1;
        set
        {
            Guard.IsLessThanOrEqualTo(value, Y2);
            if (!SetProperty(ref _y1, value))
                return;
            OnPropertyChanged(nameof(Height));
            OnPropertyChanged(nameof(YCenter));
        }
    }

    public double X2
    {
        get => _x2;
        set
        {
            Guard.IsGreaterThanOrEqualTo(value, X1);
            if (!SetProperty(ref _x2, value))
                return;
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(XCenter));
        }
    }

    public double Y2
    {
        get => _y2;
        set
        {
            Guard.IsGreaterThanOrEqualTo(value, Y1);
            if (!SetProperty(ref _y2, value))
                return;
            OnPropertyChanged(nameof(Height));
            OnPropertyChanged(nameof(YCenter));
        }
    }

    public double Width => X2 - X1;
    public double Height => Y2 - Y1;
    public double XCenter => (X1 + X2) / 2;
    public double YCenter => (Y1 + Y2) / 2;

    public BoundingBoxViewModel(Point position) : this()
    {
        _x1 = position.X;
        _x2 = position.X;
        _y1 = position.Y;
        _y2 = position.Y;
    }

    public BoundingBoxViewModel(BoundingBox bounding)
    {
        Bounding = bounding;
        _x1 = bounding.Left;
        _y1 = bounding.Top;
        _x2 = bounding.Right;
        _y2 = bounding.Bottom;
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
        _x1 = xMin;
        _x2 = xMax;
        _y1 = yMin;
        _y2 = yMax;
        OnPropertiesChanged(Properties);
    }

    public void Synchronize()
    {
        Guard.IsNotNull(Bounding);
        Bounding.SetFromTwoPositions(X1, Y1, X2, Y2);
    }

    private double _x1;
    private double _y1;
    private double _x2;
    private double _y2;

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