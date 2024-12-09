using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application;

public interface ObservableScreenshotsDataAccess : ObservableDataAccess<(ScreenshotsLibrary library, Screenshot screenshot)>;