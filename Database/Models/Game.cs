namespace Database;

public class Game : DbEntity
{
    public string State { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? CompleteDate { get; set; }

    public Account Account { get; set; }
    public ICollection<Save> Saves { get; set; }
}
