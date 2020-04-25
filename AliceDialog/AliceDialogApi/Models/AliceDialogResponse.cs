using System;
using System.Text.Json.Serialization;

namespace AliceDialogApi.Models
{
    public class AliceDialogResponse
    {
        [JsonPropertyName("response")]
        public ResponseBody Meta {get; set;}

        [JsonPropertyName("session")]
        public SessionBase Session {get; set;} 

        [JsonPropertyName("version")]
        public string Version {get;set;}
    }
}