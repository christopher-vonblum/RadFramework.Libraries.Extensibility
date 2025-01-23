using System;
using System.Diagnostics;
using NSubstitute;
using RadFramework.Abstractions.Extensibility.Pipeline;
using RadFramework.Abstractions.Extensibility.Pipeline.Asynchronous;
using RadFramework.Libraries.Extensibility.Pipeline;
using Xunit;

namespace RadFramework.Extensibility.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // plain pipeline definition objects
            PipelineDefinition<string, string> concat4As = new PipelineDefinition<string, string>();
            
            concat4As.Append<AddA>();
            concat4As.Append<AddA>();
            concat4As.Append<AddA>();
            concat4As.Append<AddA>();
            
            concat4As.Append<AddB>();
            concat4As.Append<AddB>();
            concat4As.Append<AddB>();
            concat4As.Append<AddB>();
            
            // ioc fake for pipe construction
            IServiceProvider serviceProviderMock = Substitute.For<IServiceProvider>();
         
            serviceProviderMock.GetService(Arg.Is(typeof(AddA))).Returns(new AddA());
             
            serviceProviderMock.GetService(Arg.Is(typeof(AddB))).Returns(new AddB());
            
            IPipeline<string, string> pipe = new AsyncPipeline<string, string>(concat4As, serviceProviderMock);
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            string result = (string)pipe.Process("");
            
            sw.Stop();
            
            Assert.True(result == "AAAABBBB");
        }

        private class AddA : AsyncPipeBase<string, string>
        {
            public override void Process(Func<string> input, Action<string> @continue, Action<object> @return)
            {
                @continue(input() + "A");
            }
        }

        private class AddB : AsyncPipeBase<string, string>
        {
            public override void Process(Func<string> input, Action<string> @continue, Action<object> @return)
            {
                @continue(input() + "B");
            }
        }
    }
}