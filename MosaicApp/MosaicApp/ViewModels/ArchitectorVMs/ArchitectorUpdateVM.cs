namespace MosaicApp.ViewModels.ArchitectorVMs;

public class ArchitectorUpdateVM
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public string? ImagePath { get; set; }
    public IFormFile? Image { get; set; }
    public int PositionId { get; set; }
}
