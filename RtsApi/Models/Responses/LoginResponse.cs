using Database;

namespace RtsApi;

public class LoginResponse
{
    public Account Account { get; set; }
    public string Token { get; set; }
}
