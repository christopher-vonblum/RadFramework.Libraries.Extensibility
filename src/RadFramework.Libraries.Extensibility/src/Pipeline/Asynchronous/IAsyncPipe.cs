using System;

namespace RadFramework.Abstractions.Extensibility.Pipeline.Asynchronous
{
    public interface IAsyncPipe
    {
        void Process(Func<object> input, Action<object> @continue, Action<object> @return);
    }
}