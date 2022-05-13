using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Reflection;
using System.ComponentModel;

namespace XRayAnalyzer.Objects
{
    public static class StringsResource
    {
        //language menu
        public static string MenuFile { get; private set; } = "";
        public static string SaveFile { get; private set; } = "";

        //language menu
        public static string MenuLanguage { get; private set; } = "";
        public static string LanguageSK { get; private set; } = "";
        public static string LanguageEN { get; private set; } = "";

        //spectrum processing and analysis menu
        public static string MenuView { get; private set; } = "";
        public static string SpectrumProcessingView { get; private set; } = "";
        public static string AnalysisView { get; private set; } = "";

        //data menu
        public static string DataView { get; private set; } = "";
        public static string ElementLineDataView { get; private set; } = "";
        public static string DetectorEfficiencyDataView { get; private set; } = "";
        public static string FluorescentYieldDataView { get; private set; } = "";
        public static string JumpRatioDataView { get; private set; } = "";
        public static string XrayMassCoefficientDataView { get; private set; } = "";

        //spectrum processing operations
        public static string LoadFileOperationName { get; private set; } = "";
        public static string EditPeaksOperationName { get; private set; } = "";
        public static string CalibrateDataOperationName { get; private set; } = "";
        public static string BackgroundRemovalOperationName { get; private set; } = "";
        public static string SumPeaksRemovalOperationName { get; private set; } = "";
        public static string SmoothingOperationName { get; private set; } = "";
        public static string NetExtractionOperationName { get; private set; } = "";

        //analysis operations
        public static string QualitativeOperationName { get; private set; } = "";
        public static string QuantitativeOperationName { get; private set; } = "";

        //filter operations
        public static string EqualFilterOperation { get; private set; } = "";
        public static string RangeFilterOperation { get; private set; } = "";
        public static string ContainsFilterOperation { get; private set; } = "";
        public static string StartsWithFilterOperation { get; private set; } = "";
        public static string EndsWithFilterOperation { get; private set; } = "";

        public static void ChangeLanguage(string language)
        {
            foreach (string file in Directory.EnumerateFiles(Properties.Settings.Default.LanguagePath, "*.json"))
            {
                string fileLanguage = Path.GetFileNameWithoutExtension(file);
                if (fileLanguage.ToLower() == language.ToLower())
                {
                    Dictionary<string, string>? contents = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(file));

                    List<PropertyInfo> propertiesInfo = typeof(StringsResource).GetProperties().ToList();
                    foreach (PropertyInfo propertyInfo in propertiesInfo)
                    {
                        propertyInfo.SetValue(null, contents?.GetValueOrDefault(propertyInfo.Name));
                        OnStaticPropertyChanged(propertyInfo.Name);
                    }

                    //Properties.Settings.Default.Language = language;
                    //Properties.Settings.Default.Save();

                    break;
                }
            }

        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged(string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}
