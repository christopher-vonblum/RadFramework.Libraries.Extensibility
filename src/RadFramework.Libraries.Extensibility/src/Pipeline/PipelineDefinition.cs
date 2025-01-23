using System;
using System.Collections.Generic;

namespace RadFramework.Libraries.Extensibility.Pipeline
{
    [Serializable]
    public class PipelineDefinition<TIn, TOut> : IPipelineDefinition
    {
        public IEnumerable<PipeDefinition> Definitions => pipes;
        private List<PipeDefinition> pipes = new List<PipeDefinition>();

        public PipelineDefinition(IEnumerable<PipeDefinition> definitions)
        {
            pipes = new List<PipeDefinition>(definitions);
        }

        public PipelineDefinition()
        {
        }
        
        public void InsertAfter<TPipe>(string afterKey, string key = null)
        {
            int afterIndex = pipes.FindIndex(definition => definition.Key == afterKey);
            pipes.Insert(afterIndex + 1, new PipeDefinition(typeof(TPipe), key));
        }
        
        public void InsertAfter<TAfter, TPipe>(string key = null)
        {
            int afterIndex = pipes.FindIndex(definition => definition.Type == typeof(TAfter));
            pipes.Insert(afterIndex + 1, new PipeDefinition(typeof(TPipe), key));
        }

        public void InsertBefore<TPipe>(string beforeKey, string key = null)
        {
            int afterIndex = pipes.FindIndex(definition => definition.Key == beforeKey);
            pipes.Insert(afterIndex - 1, new PipeDefinition(typeof(TPipe), key));
        }
        
        public void InsertBefore<TBefore, TPipe>(string key = null)
        {
            int afterIndex = pipes.FindIndex(definition => definition.Type == typeof(TBefore));
            pipes.Insert(afterIndex - 1, new PipeDefinition(typeof(TPipe), key));
        }

        public void Replace<TReplace, TPipe>(string key = null)
        {
            int replaceIndex = pipes.FindIndex(definition => definition.Type == typeof(TReplace));
            pipes.RemoveAt(replaceIndex);
            pipes.Insert(replaceIndex, new PipeDefinition(typeof(TPipe), key));
        }
        
        public void Prepend<TPipe>(string key = null)
        {
            pipes.Insert(0, new PipeDefinition(typeof(TPipe), key));
        }
        
        public void Append<TPipe>(string key = null)
        {
            pipes.Add(new PipeDefinition(typeof(TPipe), key));
        }
    }

    public interface IPipelineDefinition
    {
    }
}