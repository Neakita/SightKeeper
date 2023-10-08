using FlakeId;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace SightKeeper.Data;

public sealed class FlakeIdGenerator : ValueGenerator<Id>
{
    public override bool GeneratesTemporaryValues => false;
    public override Id Next(EntityEntry entry) => Id.Create();
}