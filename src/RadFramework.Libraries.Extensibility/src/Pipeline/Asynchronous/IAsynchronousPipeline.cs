using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RadFramework.Libraries.Extensibility.Pipeline;

namespace RadFramework.Abstractions.Extensibility.Pipeline.Asynchronous
{
    public class IAsynchronousPipeline<TIn, TOut> : IPipeline<TIn, TOut>
    {
        private readonly IServiceProvider _serviceProvider;
        public LinkedList<IAsynchronousPipe> definitions;
        
        public IAsynchronousPipeline(PipelineDefinition<TIn, TOut> definition, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            definitions = new LinkedList<IAsynchronousPipe>(definition.Definitions.Select(CreatePipe));
        }
        private IAsynchronousPipe CreatePipe(PipeDefinition def)
        {
            return (IAsynchronousPipe) _serviceProvider.GetService(def.Type);
        }

        public TOut Process(TIn input)
        {
            PipeContext pipelineContext = new PipeContext();
            
            List<PipeContext> contexts = new List<PipeContext>();
            
            Thread first = new Thread(() =>
            {
                PipeContext p = new PipeContext();
                if (definitions.First.Next != null)
                {
                    CreateThread(p, definitions.First.Next, pipelineContext, contexts);
                }
                else
                {
                    definitions.First.Value.Process(() => input,
                        o =>  { Return(pipelineContext, o); },
                        o => { Return(pipelineContext, o); });
                    return;
                }
                
                definitions.First.Value.Process(() => input,
                    o =>{ Return(p, o); },
                    o => { Return(pipelineContext, o); });
            });
            first.Start();

            pipelineContext.PreviousReturnedValue.WaitOne();
            
            foreach (var pipeContext in contexts.Where(t => t.WaitingForInput))
            {
                pipeContext.Thread.Abort();
            }
            
            return (TOut)pipelineContext.ReturnValue;
        }

        private static void Return(PipeContext pipelineContext, object o)
        {
            pipelineContext.ReturnValue = o;
            pipelineContext.PreviousReturnedValue.Set();
        }

        private void CreateThread(PipeContext previousContext, LinkedListNode<IAsynchronousPipe> current,
            PipeContext pipelineContext, List<PipeContext> contexts)
        {
            Thread pipeThread = new Thread(() =>
                {
                    PipeContext currentContext = new PipeContext();
                    
                    currentContext.Thread = Thread.CurrentThread;
                    
                    contexts.Add(currentContext);
                    
                    if (current.Next != null)
                    {
                        CreateThread(currentContext, current.Next, pipelineContext, contexts);
                    }
                    else
                    {
                        current.Value.Process(() =>
                            {
                                previousContext.PreviousReturnedValue.WaitOne();
                                currentContext.WaitingForInput = false;
                                return previousContext.ReturnValue;
                            },
                            o => { Return(pipelineContext, o); },
                            o => { Return(pipelineContext, o); });
                        return;
                    }

                    current.Value.Process(() =>
                        {
                            previousContext.PreviousReturnedValue.WaitOne();
                            currentContext.WaitingForInput = false;
                            return previousContext.ReturnValue;
                        },
                        o =>{ Return(currentContext, o); },
                        o => { Return(pipelineContext, o); });
                });
            
            pipeThread.Start();
        }
    }

    class PipeContext
    {
        public ManualResetEvent PreviousReturnedValue { get; } = new ManualResetEvent(false);
        public object ReturnValue { get; set; }
        public bool WaitingForInput { get; set; } = true;
        public Thread Thread { get; set; }
    }
}