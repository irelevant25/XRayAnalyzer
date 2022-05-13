using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.Communication.Analysis;
using XRayAnalyzer.MVVM.Model.Communication.BackgroundRemoval;
using XRayAnalyzer.MVVM.Model.Communication.Calibration;
using XRayAnalyzer.MVVM.Model.Communication.MCA;

namespace XRayAnalyzer.Objects
{
    /// <summary>
    /// Shared model across views
    /// </summary>
    public class MainModel : PropertyChangedBaseModel
    {
        public JSON Data { get; set; } = new();

        /// <summary>
        /// Lazy and thread safe singleton
        /// </summary>

        [JsonIgnore]
        private static readonly Lazy<MainModel> lazy = new (() => new MainModel());

        [JsonIgnore]
        public static MainModel Instance { get { return lazy.Value; } }

        private MainModel()
        {
        }
    }
}
