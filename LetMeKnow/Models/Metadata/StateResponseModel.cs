using Newtonsoft.Json;
using System.Collections.Generic;

namespace LetMeKnow.Models.Metadata
{
    public class State
    {
        [JsonProperty("state_id")]
        public int StateId { get; set; }

        [JsonProperty("state_name")]
        public string StateName { get; set; }
    }

    public class StatesResponseModel
    {
        [JsonProperty("states")]
        public List<State> States { get; set; }

        [JsonProperty("ttl")]
        public int Ttl { get; set; }
    }


}
