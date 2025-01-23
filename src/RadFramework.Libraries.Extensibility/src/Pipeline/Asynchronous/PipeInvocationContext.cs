using System.Threading;

namespace RadFramework.Abstractions.Extensibility.Pipeline.Asynchronous
{
    public class PipeInvocationContext
    {
        private readonly PipeInvocationContext _parent;
        public object Result { get; set; }
        public ManualResetEvent ValueReturned { get; } = new ManualResetEvent(false);
        public ManualResetEvent PipelineResultReturned { get; } = new ManualResetEvent(false);
        public PipeInvocationContext()
        {
            PipelineResultReturned = new ManualResetEvent(false);
        }

        public PipeInvocationContext(PipeInvocationContext parent)
        {
            _parent = parent;
            PipelineResultReturned = parent.PipelineResultReturned;
        }

        public void Return()
        {
            _parent.Result = this.Result;
            _parent.PipelineResultReturned.Set();
        }
    }
}