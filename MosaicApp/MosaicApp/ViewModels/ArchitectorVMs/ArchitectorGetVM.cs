using MosaicApp.ViewModels.PositionVMs;

namespace MosaicApp.ViewModels.ArchitectorVMs;

public class ArchitectorGetVM
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public string? ImagePath { get; set; }
    public PositionGetVM Position { get; set; }
}
