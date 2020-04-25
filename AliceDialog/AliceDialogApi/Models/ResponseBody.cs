using System.Text.Json.Serialization;

namespace AliceDialogApi.Models
{
    public class ResponseBody
    {
        [JsonPropertyName("text")]
        public string Text {get; set;}

        [JsonPropertyName("tts")]
        public string Tts {get; set;} 

        [JsonPropertyName("end_session")]
        public bool IsSessionEnd {get;set;}
    }
}