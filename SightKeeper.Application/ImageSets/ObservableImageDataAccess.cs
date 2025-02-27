using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets;

public interface ObservableImageDataAccess : ObservableDataAccess<(ImageSet library, Image image)>;