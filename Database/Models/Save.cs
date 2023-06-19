namespace Database;

public class Save : DbEntity
{
    public int Order { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string MapСhanges { get; set; }
    public double Level { get; set; }
    public double Money { get; set; }
    public double Bits { get; set; }
}
