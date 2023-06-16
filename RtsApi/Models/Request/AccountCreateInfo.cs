using Contracts;

namespace RtsApi;
public class AccountCreateInfo : IValidatable
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public bool IsValid
    {
        get { try { this.Validate(); return true; } catch { return false; } }
    }

    public void Validate()
    {
        if (string.IsNullOrEmpty(Name)) throw new Exception("��� �� �������");
        if (string.IsNullOrEmpty(Email)) throw new Exception("����� �� �������");
        if (!Email.Contains('@') || !Email.Contains('.')) throw new Exception("����� ����� �������� ������");
        RolesParser.Parse(Role);
        //if (string.IsNullOrEmpty(Password) || Password.Count() < 6) throw new Exception("������ ����� ����� 6 ��������");
    }
}