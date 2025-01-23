namespace RadFramework.Abstractions.Extensibility.Pipeline.Synchronous
{
    public interface ISyncPipe
    {
        object Process(object input);
    }
}