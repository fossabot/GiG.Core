using BenchmarkDotNet.Attributes;
using GiG.Core.Models;
using GiG.Core.Validation.FluentValidation.Web.Extensions;
using System.Collections.Generic;

namespace GiG.Core.Benchmarks.Web
{
    public class FluentValidationSerializeBenchmarks
    {
        private readonly ErrorResponse _smallErrorResponse;
        private readonly ErrorResponse _largeErrorResponse;

        public FluentValidationSerializeBenchmarks()
        {
            _smallErrorResponse = new ErrorResponse
            {
                Errors = new Dictionary<string, List<string>>
                {
                    {"Address", new List<string> {"Address field is required."}}
                }
            };

            _largeErrorResponse = new ErrorResponse
            {
                Errors = new Dictionary<string, List<string>>
                {
                    {"Name", new List<string> {"Name field should contain at least 2 characters.", "Name field cannot contain only numbers."}},
                    {"Surname", new List<string> {"Surname field is required."}},
                    {"Address", new List<string> {"Address field is required."}}
                }
            };
        }

        [Benchmark]
        public void Serialize_SmallResponse()
        {
            _smallErrorResponse.Serialize(null);
        }

        [Benchmark]
        public void Serialize_LargeResponse()
        {
            _largeErrorResponse.Serialize(null);
        }
    }
}
