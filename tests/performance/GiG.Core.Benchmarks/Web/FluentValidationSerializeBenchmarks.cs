using BenchmarkDotNet.Attributes;
using GiG.Core.Web.FluentValidation.Extensions;
using GiG.Core.Web.FluentValidation.Internal;
using System.Collections.Generic;

namespace GiG.Core.Benchmarks.Web
{
    public class FluentValidationSerializeBenchmarks
    {
        private readonly ValidationResponse _smallValidationResponse;
        private readonly ValidationResponse _largeValidationResponse;

        public FluentValidationSerializeBenchmarks()
        {
            _smallValidationResponse = new ValidationResponse
            {
                Errors = new Dictionary<string, List<string>>
                {
                    {"Address", new List<string> {"Address field is required."}}
                }
            };

            _largeValidationResponse = new ValidationResponse
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
            _smallValidationResponse.Serialize(null);
        }

        [Benchmark]
        public void Serialize_LargeResponse()
        {
            _largeValidationResponse.Serialize(null);
        }
    }
}
