using System;
using System.Collections.Generic;
using System.Linq;
using RadFramework.Libraries.Extensibility.Pipeline;

namespace RadFramework.Abstractions.Extensibility.Pipeline.Synchronous
{
    public class SynchronousPipeline<TIn, TOut> : IPipeline<TIn, TOut>
    {
        private readonly IServiceProvider _serviceProvider;
        public LinkedList<ISynchronousPipe> definitions;

        public SynchronousPipeline(PipelineDefinition<TIn, TOut> definition, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            definitions = new LinkedList<ISynchronousPipe>(definition.Definitions.Select(CreatePipe));
        }
        
        public SynchronousPipeline(IEnumerable<ISynchronousPipe> definitions)
        {
            definitions = new LinkedList<ISynchronousPipe>(definitions);
        }

        private ISynchronousPipe CreatePipe(PipeDefinition def)
        {
            return (ISynchronousPipe) _serviceProvider.GetService(def.Type);
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