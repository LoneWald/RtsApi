namespace Contracts;

public enum Roles
{
    ADMIN = 0,
    STUDENT = 1,
    TEACHER = 2,
}

public static class RolesParser
{
    public static Roles Parse(string str)
    {
        return str.ToUpper() switch
        {
            "ADMIN" => Roles.ADMIN,
            "STUDENT" => Roles.STUDENT,
            "TEACHER" => Roles.TEACHER,
            _ => throw new Exception("Неизвестная роль")
        };
    }
}