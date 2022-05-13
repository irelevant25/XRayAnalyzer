using XRayAnalyzer.Objects.Enums;

namespace XRayAnalyzer.MVVM.Model
{
    class Response : PropertyChangedBaseModel
    {
        public ResponseType Type { get; set; }
        public string Message { get; set; } = string.Empty;

        public Response(ResponseType type, string? message)
        {
            Type = type;
            if (!string.IsNullOrEmpty(message))
            {
                Message = message;
            }
        }

        public Response Clone()
        {
            return new Response(Type, Message);
        }
    }
}
