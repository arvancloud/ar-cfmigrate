namespace ArvanCloudLib.Models
{
    public class BaseArvanModel<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}