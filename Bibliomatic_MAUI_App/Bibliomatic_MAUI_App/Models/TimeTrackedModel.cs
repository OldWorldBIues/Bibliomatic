using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Models
{
    public class TimeTrackedModel
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonIgnore]
        public string CreatedAtFormat { get => CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm"); }

        [JsonIgnore]
        public string UpdatedAtFormat { get => UpdatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm"); }
    }
}
