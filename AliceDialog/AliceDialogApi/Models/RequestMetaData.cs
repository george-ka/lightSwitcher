using System.Text.Json.Serialization;

namespace AliceDialogApi.Models
{
    public class RequestMetaData
    {
        [JsonPropertyName("locale")]
        public string Locale { get; set; }
        
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
    }
}