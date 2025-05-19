namespace MosaicApp.Models;

public class Position : BaseEntity
{
    public string Name { get; set; }
    public List<Architector> Architectors { get; set; }
}
