using Newtonsoft.Json;

namespace Easyyyyy_Reborn.Models
{
    public class Configuration
    {
        [JsonProperty("toggle_mode")]
        public bool IsToggleMode { get; set; }

        [JsonProperty("default_clicks")]
        public bool IsDefaultClicks { get; set; }

        [JsonProperty("is_left_click")]
        public bool IsLeftClick { get; set; }

        [JsonProperty("count_cps")]
        public int CountClicksPerSecond { get; set; }

        [JsonProperty("enabled_random")]
        public bool IsEnabledRandom { get; set; }

        [JsonProperty("int_bind_key")]
        public int IntBindKey { get; set; }

        [JsonProperty("bind_key")]
        public string? BindKey { get; set; }
    }
}
