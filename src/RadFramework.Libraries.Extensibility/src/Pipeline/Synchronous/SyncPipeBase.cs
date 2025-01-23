namespace RadFramework.Abstractions.Extensibility.Pipeline.Synchronous
{
    public abstract class SyncPipeBase<TInput, TOutput> : ISyncPipe
    {
        public object Process(object input)
        {
            return Process((TInput) input);
        }

        public abstract TOutput Process(TInput input);
    }
}