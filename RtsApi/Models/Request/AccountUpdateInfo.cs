using Contracts;

namespace RtsApi;
public class AccountUpdateInfo : IValidatable
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool? IsActive { get; set; }
    public bool IsValid
    {
        get { try { this.Validate(); return true; } catch { return false; } }
    }

    public void Validate()
    {
        if (Id == 0) throw new Exception("Неверно указан ID");
    }
}