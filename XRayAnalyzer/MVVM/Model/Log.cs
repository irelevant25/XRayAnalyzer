using System;
using XRayAnalyzer.Objects.Enums;

namespace XRayAnalyzer.MVVM.Model
{
    class Log : PropertyChangedBaseModel
    {
        public LogType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
    }
}
