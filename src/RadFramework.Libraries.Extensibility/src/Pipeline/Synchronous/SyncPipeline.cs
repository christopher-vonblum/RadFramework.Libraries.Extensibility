using System;
using System.Collections.Generic;
using System.Linq;
using RadFramework.Libraries.Extensibility.Pipeline;

namespace RadFramework.Abstractions.Extensibility.Pipeline.Synchronous
{
    public class SyncPipeline<TIn, TOut> : IPipeline<TIn, TOut>
    {
        private readonly IServiceProvider _serviceProvider;
        public LinkedList<ISyncPipe> definitions;

        public SyncPipeline(PipelineDefinition<TIn, TOut> definition, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            definitions = new LinkedList<ISyncPipe>(definition.Definitions.Select(CreatePipe));
        }

        private ISyncPipe CreatePipe(PipeDefinition def)
        {
            return (ISyncPipe) _serviceProvider.GetService(def.Type);
        }

        public TOut Process(TIn input)
        {
            object result = input;
            
            foreach (var pipe in definitions)
            {
                result = pipe.Process(result);
            }

            return (TOut) result;
        }
    }
}