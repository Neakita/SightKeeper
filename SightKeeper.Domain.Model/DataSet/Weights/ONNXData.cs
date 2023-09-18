namespace SightKeeper.Domain.Model;

public sealed class ONNXData : WeightsData
{
    public ONNXData(byte[] content) : base(content)
    {
    }
}