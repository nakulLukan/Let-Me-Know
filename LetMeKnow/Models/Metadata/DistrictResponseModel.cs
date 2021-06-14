using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetMeKnow.Models.Metadata
{
    public class District
    {
        [JsonProperty("state_id")]
        public int StateId { get; set; }

        [JsonProperty("district_id")]
        public int DistrictId { get; set; }

        [JsonProperty("district_name")]
        public string DistrictName { get; set; }

        [JsonProperty("district_name_l")]
        public string DistrictNameL { get; set; }
    }

    public class DistrictResponseModel
    {
        [JsonProperty("districts")]
        public List<District> Districts { get; set; }

        [JsonProperty("ttl")]
        public int Ttl { get; set; }
    }
}
