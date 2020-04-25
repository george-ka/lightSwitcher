using System.Text.Json.Serialization;

namespace AliceDialogApi.Models
{
    public class SessionBase
    {
        [JsonPropertyName("message_id")]
        public long MessageId {get; set;}

        [JsonPropertyName("session_id")]
        public string SessionId {get; set;}

        [JsonPropertyName("user_id")]
        public string UserId {get; set;}     
    }
}