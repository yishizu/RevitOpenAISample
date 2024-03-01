using System.Collections.Generic;
using Newtonsoft.Json;

namespace AiCorb.Data
{
    public class DTypeData
    {
        [JsonProperty("dtype")]
        public List<string> DType { get; set; }

        [JsonProperty("prob")]
        public List<double> Prob { get; set; }
    }
}