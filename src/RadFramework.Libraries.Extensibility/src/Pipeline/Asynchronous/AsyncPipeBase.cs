using System;

namespace RadFramework.Abstractions.Extensibility.Pipeline.Asynchronous
{
    public abstract class AsyncPipeBase<TIn, TOut> : IAsyncPipe
    {
        public void Process(Func<object> input, Action<object> @continue, Action<object> @return)
        {
            Process(() => (TIn)input(), o => @continue(o), o => @return(o));
        }
        
        public abstract void Process(Func<TIn> input, Action<TOut> @continue, Action<object> @return);
    }
}