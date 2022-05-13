using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Services;

namespace XRayAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            StringsResource.ChangeLanguage(XRayAnalyzer.Properties.Settings.Default.Language);
            PythonService.Instance.Run();
            Dataset.LoadDatasets();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            StartupUri = new Uri("./MVVM/View/MainView.xaml", UriKind.Relative);
        }
    }
}
