using MosaicApp.ViewModels.PositionVMs;

namespace MosaicApp.Models;

public class Architector : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public string? ImagePath { get; set; }

    public int PositionId { get; set; }
    public Position Position { get; set; }
}
