namespace Contracts;

public class Error
{
    public Error(Guid id, string message)
    {
        Id = id;
        Message = message;
    }

    public Guid Id { get; set; }
    public string Message { get; set; }
}