namespace Database;

public class Account : DbEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name{ get; set; }
    public string Role { get; set; }
    public bool IsActive{ get; set; }
}
