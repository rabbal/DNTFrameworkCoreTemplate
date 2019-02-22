using Newtonsoft.Json;

namespace DNTFrameworkCoreTemplateAPI.API.Authentication
{
    public class Token
    {
        [JsonProperty("token")]
        public string Value { get; set; }
    }

}