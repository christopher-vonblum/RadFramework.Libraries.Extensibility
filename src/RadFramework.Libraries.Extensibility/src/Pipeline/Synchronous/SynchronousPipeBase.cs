namespace RadFramework.Abstractions.Extensibility.Pipeline.Synchronous
{
    public abstract class SynchronousPipeBase<TInput, TOutput> : ISynchronousPipe
    {
        public object Process(object input)
        {
            return Process((TInput) input);
        }

        public abstract TOutput Process(TInput input);
    }
}