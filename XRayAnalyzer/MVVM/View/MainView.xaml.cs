using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XRayAnalyzer.MVVM.Model;
using ScottPlot;
using XRayAnalyzer.Services;
using XRayAnalyzer.MVVM.ViewModel;
using System.Reflection;
using XRayAnalyzer.Objects;
using System.ComponentModel;
using XRayAnalyzer.MVVM.View.SpectrumProcessing;
using XRayAnalyzer.MVVM.View.DataViewer;
using XRayAnalyzer.MVVM.View.Analysis;
using Microsoft.Win32;
using System.IO;

namespace XRayAnalyzer.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private MainViewModel ViewModel { get; set; }
        private static FieldInfo _menuDropAlignmentField;

        public MainView()
        {
            _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            System.Diagnostics.Debug.Assert(_menuDropAlignmentField != null);

            EnsureStandardPopupAlignment();
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;

            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            InitializeComponent();
            Init();
        }

        private static void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            EnsureStandardPopupAlignment();
        }

        private static void EnsureStandardPopupAlignment()
        {
            if (SystemParameters.MenuDropAlignment && _menuDropAlignmentField != null)
            {
                _menuDropAlignmentField.SetValue(null, false);
            }
        }

        private void Init()
        {
            List<PropertyInfo> propertiesInfo = typeof(StringsResource).GetProperties().ToList();
            foreach (PropertyInfo propertyInfo in propertiesInfo)
            {
                if (propertyInfo.Name.StartsWith("Language")) MenuLanguage.MenuItemAdd(propertyInfo.Name, MenuItem_LanguageItem_Click, propertyInfo.Name);
            }

            ViewModel.SelectedView = MenuView.MenuItemAdd(nameof(StringsResource.SpectrumProcessingView), Item_ViewItem_Click, new SpectrumProcessingView()).Tag;
            MenuView.MenuItemAdd(nameof(StringsResource.AnalysisView), Item_ViewItem_Click, new AnalysisView());

            MenuFile.MenuItemAdd(nameof(StringsResource.SaveFile), Item_SaveFile_Click);

            DataView.MenuItemAdd(nameof(StringsResource.ElementLineDataView), Item_ViewItem_Click, new ElementLineDataView());
            DataView.MenuItemAdd(nameof(StringsResource.DetectorEfficiencyDataView), Item_ViewItem_Click, new DetectorEfficiencyDataView());
            DataView.MenuItemAdd(nameof(StringsResource.FluorescentYieldDataView), Item_ViewItem_Click, new FluorescentYieldDataView());
            DataView.MenuItemAdd(nameof(StringsResource.JumpRatioDataView), Item_ViewItem_Click, new JumpRatioDataView());
            DataView.MenuItemAdd(nameof(StringsResource.XrayMassCoefficientDataView), Item_ViewItem_Click, new XrayMassCoefficientDataView());
        }

        private void Item_SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new()
            {
                Filter = "*.json|*.json"
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                File.WriteAllText(dialog.FileName, MainModel.Instance.Data.JsonSerialize(true));
                MessageBox.Show("File was sucessfully saved.", "File save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MenuItem_LanguageItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = (MenuItem)sender;
            StringsResource.ChangeLanguage(clickedItem.Tag.ToString() ?? "");

            foreach (MenuItem item in MenuLanguage.Items.Cast<MenuItem>())
            {
                item.IsChecked = false;
            }

            clickedItem.IsChecked = true;
        }

        private void Item_ViewItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = (MenuItem)sender;
            StringsResource.ChangeLanguage(clickedItem.Tag.ToString() ?? "");

            foreach (MenuItem item in MenuView.Items.Cast<MenuItem>()) item.IsChecked = item.Tag.ToString() == Properties.Settings.Default.View;
            foreach (MenuItem item in DataView.Items.Cast<MenuItem>()) item.IsChecked = item.Tag.ToString() == Properties.Settings.Default.View;

            clickedItem.IsChecked = true;
            ViewModel.SelectedView = clickedItem.Tag;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }
    }
}
