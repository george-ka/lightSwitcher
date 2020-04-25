using System.Text.Json.Serialization;

namespace AliceDialogApi.Models
{
    public class RequestBody
    {
        [JsonPropertyName("command")]
        public string Command {get; set;}

        [JsonPropertyName("original_utterance")]
        public string OriginalUtterance {get; set;}

        [JsonPropertyName("type")]
        public InputType Type {get; set;}

    }
}