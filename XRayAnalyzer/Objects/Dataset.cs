using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.Communication.Data;
using XRayAnalyzer.Services;

namespace XRayAnalyzer.Objects
{
    internal static class Dataset
    {
        public static ReadOnlyCollection<ElementLine>? ElementsLines { get; private set; }
        public static ReadOnlyCollection<ElementInfo>? ElementsInfo { get; private set; }
        public static ReadOnlyCollection<XrayMassCoefficient>? XrayMassCoefficients { get; private set; }
        public static ReadOnlyCollection<DetectorEfficiency>? DetectorEfficiencies { get; private set; }
        public static ReadOnlyCollection<FluorescentYield>? FluorescentYields { get; private set; }
        public static ReadOnlyCollection<JumpRatio>? JumpRatios { get; private set; }

        public static void LoadDatasets()
        {
            LoadElementsLines(Path.Combine(Directory.GetCurrentDirectory(), @"Datasets\elements_lines.json"));
            LoadElementsInfo(Path.Combine(Directory.GetCurrentDirectory(), @"Datasets\elements_info.json"));
            LoadXrayMassCoeficients(Path.Combine(Directory.GetCurrentDirectory(), @"Datasets\xray_mass_ceoficient.json"));
            LoadDetectorEfficiencies(Path.Combine(Directory.GetCurrentDirectory(), @"Datasets\detector_efficiency.json"));
            LoadFluorescentYields(Path.Combine(Directory.GetCurrentDirectory(), @"Datasets\fluorescent_yield.json"));
            LoadJumpRatios(Path.Combine(Directory.GetCurrentDirectory(), @"Datasets\jump_ratio.json"));
        }

        private static void LoadElementsLines(string fileName)
        {
            ElementLineResponse? response = PythonService.Instance.GetData<DataProperties, ElementLineResponse>(new DataProperties()
            {
                FileName = fileName
            });
            if (response?.ElementsLines != null)
            {
                ElementsLines = new ReadOnlyCollection<ElementLine>(response.ElementsLines);
            }
        }

        private static void LoadElementsInfo(string fileName)
        {
            ElementInfoResponse? response = PythonService.Instance.GetData<DataProperties, ElementInfoResponse>(new DataProperties()
            {
                FileName = fileName
            });
            if (response?.ElementsInfo != null)
            {
                ElementsInfo = new ReadOnlyCollection<ElementInfo>(response.ElementsInfo);
            }
        }

        private static void LoadXrayMassCoeficients(string fileName)
        {
            XrayMassCoefficientResponse? response = PythonService.Instance.GetData<DataProperties, XrayMassCoefficientResponse>(new DataProperties()
            {
                FileName = fileName
            });
            if (response?.XrayMassCoefficients != null)
            {
                XrayMassCoefficients = new ReadOnlyCollection<XrayMassCoefficient>(response.XrayMassCoefficients);
            }
        }

        private static void LoadDetectorEfficiencies(string fileName)
        {
            DetectorEfficiencyResponse? response = PythonService.Instance.GetData<DataProperties, DetectorEfficiencyResponse>(new DataProperties()
            {
                FileName = fileName
            });
            if (response?.DetectorEfficiencies != null)
            {
                DetectorEfficiencies = new ReadOnlyCollection<DetectorEfficiency>(response.DetectorEfficiencies);
            }
        }

        private static void LoadFluorescentYields(string fileName)
        {
            FluorescentYieldResponse? response = PythonService.Instance.GetData<DataProperties, FluorescentYieldResponse>(new DataProperties()
            {
                FileName = fileName
            });
            if (response?.FluorescentYields != null)
            {
                FluorescentYields = new ReadOnlyCollection<FluorescentYield>(response.FluorescentYields);
            }
        }

        private static void LoadJumpRatios(string fileName)
        {
            JumpRatioResponse? response = PythonService.Instance.GetData<DataProperties, JumpRatioResponse>(new DataProperties()
            {
                FileName = fileName
            });
            if (response?.JumpRatios != null)
            {
                JumpRatios = new ReadOnlyCollection<JumpRatio>(response.JumpRatios);
            }
        }
    }
}
