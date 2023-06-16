namespace Contracts;
public class Answer<T>
{
    public Answer(Error error)
    {
        Error = error;
    }
    public Answer(T data)
    {
        Data = data;
    }

    public T? Data { get; set; }
    public Error? Error { get; set; }
}