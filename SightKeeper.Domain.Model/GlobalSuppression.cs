using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("NDepend", "ND1207:NonStaticClassesShouldBeInstantiatedOrTurnedToStatic", Target="SightKeeper.Domain.Model.DataSet")]
[assembly: SuppressMessage("NDepend", "ND1207:NonStaticClassesShouldBeInstantiatedOrTurnedToStatic", Target="SightKeeper.Domain.Model.Game")]
[assembly: SuppressMessage("NDepend", "ND1207:NonStaticClassesShouldBeInstantiatedOrTurnedToStatic", Target="SightKeeper.Domain.Model.Profile")]
[assembly: SuppressMessage("NDepend", "ND1701:PotentiallyDeadMethods", Target="SightKeeper.Domain.Model.DetectorItem..ctor()", Justification="EF Core requires empty constructor or constructor without reference parameters")]
[assembly: SuppressMessage("NDepend", "ND1701:PotentiallyDeadMethods", Target="SightKeeper.Domain.Model.Profile..ctor()", Justification="EF Core requires empty constructor or constructor without reference parameters")]
[assembly: SuppressMessage("NDepend", "ND1701:PotentiallyDeadMethods", Target="SightKeeper.Domain.Model.ProfileItemClass..ctor()", Justification="EF Core requires empty constructor or constructor without reference parameters")]
[assembly: SuppressMessage("NDepend", "ND1701:PotentiallyDeadMethods", Target="SightKeeper.Domain.Model.Screenshot..ctor()", Justification="EF Core requires empty constructor or constructor without reference parameters")]
[assembly: SuppressMessage("NDepend", "ND1701:PotentiallyDeadMethods", Target="SightKeeper.Domain.Model.Weights..ctor()", Justification="EF Core requires empty constructor or constructor without reference parameters")]
[assembly: SuppressMessage("NDepend", "ND1702:PotentiallyDeadFields", Target="SightKeeper.Domain.Model.Entity._id", Justification="Used by EF Core")]