using System.Text.Json.Serialization;

namespace SightKeeper.Application.Training.COCO;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(COCODataSet))]
internal sealed partial class COCODataSetSourceGenerationContext : JsonSerializerContext;