namespace Database;

public class Log : DbEntity
{
    public DateTime Date { get; set; }
    public string Text { get; set; }
}
