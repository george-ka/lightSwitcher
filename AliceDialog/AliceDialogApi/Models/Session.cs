using System.Text.Json.Serialization;

namespace AliceDialogApi.Models
{
    public class Session : SessionBase
    {
        [JsonPropertyName("new")]
        public bool IsNew {get; set;}

        [JsonPropertyName("skill_id")]
        public string SkillId {get; set;}
    }
}