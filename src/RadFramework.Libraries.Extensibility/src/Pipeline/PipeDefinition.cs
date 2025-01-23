using System;

namespace RadFramework.Libraries.Extensibility.Pipeline
{
    [Serializable]
    public class PipeDefinition
    {
        public string Key { get; }
        
        public Type Type { get; }

        public PipeDefinition(Type type, string key = null)
        {
            Type = type;
            Key = key ?? type.FullName;
        }
    }
}