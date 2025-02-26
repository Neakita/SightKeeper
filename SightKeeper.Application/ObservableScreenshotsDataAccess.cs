using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public interface ObservableScreenshotsDataAccess : ObservableDataAccess<(ImageSet library, Image screenshot)>;