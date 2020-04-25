using System;
using System.Text.Json.Serialization;

namespace AliceDialogApi.Models
{
    public class AliceDialogRequest
    {
        [JsonPropertyName("meta")]
        public RequestMetaData Meta {get; set;}

        [JsonPropertyName("request")]
        public RequestBody Request {get; set;}
        
        [JsonPropertyName("session")]
        public Session Session {get; set;}

        [JsonPropertyName("version")]
        public string Version {get;set;}
    }
}