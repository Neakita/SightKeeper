using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public interface ObservableScreenshotsDataAccess : ObservableDataAccess<(ScreenshotsLibrary library, Screenshot screenshot)>;