using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace my_clean_way.data
{
    public class PaginableResponse<T>
    {
        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("results")]
        public List<T> Results { get; set; }

        [JsonProperty("total_results")]
        public long TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }
    }
}
