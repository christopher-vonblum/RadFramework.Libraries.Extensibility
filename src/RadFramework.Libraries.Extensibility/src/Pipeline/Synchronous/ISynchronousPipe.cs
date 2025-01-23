namespace RadFramework.Abstractions.Extensibility.Pipeline.Synchronous
{
    public interface ISynchronousPipe
    {
        object Process(object input);
    }
}