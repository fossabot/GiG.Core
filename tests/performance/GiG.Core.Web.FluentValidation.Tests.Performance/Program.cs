using BenchmarkDotNet.Running;

namespace GiG.Core.Web.FluentValidation.Tests.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<JsonWriterTest>(); 

        }
    }
}
