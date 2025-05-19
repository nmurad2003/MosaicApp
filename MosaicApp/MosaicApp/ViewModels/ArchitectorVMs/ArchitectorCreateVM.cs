using MosaicApp.ViewModels.PositionVMs;

namespace MosaicApp.ViewModels.ArchitectorVMs;

public class ArchitectorCreateVM
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
    public int PositionId { get; set; }
}
