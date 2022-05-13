using System.Text.Json.Serialization;

namespace XRayAnalyzer.MVVM.Model.Communication.MCA
{
    internal class MCAProperties : IPropertiesBaseInterface
    {
        [JsonIgnore]
        public string FunctionName { get; } = "load_mca";

        [JsonPropertyName("full_file_name")]
        public string? FileName { get; set; }
    }
}
