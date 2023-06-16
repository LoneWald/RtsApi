using Contracts;

namespace RtsApi;
public class LoginModel : IValidatable
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsValid
    {
        get { try { this.Validate(); return true; } catch { return false; } }
    }

    public void Validate()
    {
        if (string.IsNullOrEmpty(Email)) { throw new Exception("Логин не указан"); }
        if (string.IsNullOrEmpty(Password)) { throw new Exception("Пароль не указан"); }
    }
}