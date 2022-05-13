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
using XRayAnalyzer.MVVM.Model.DataTable;
using XRayAnalyzer.MVVM.ViewModel.DataViewer;
using XRayAnalyzer.Objects;

namespace XRayAnalyzer.MVVM.View.DataViewer
{
    /// <summary>
    /// Interaction logic for EfficiencyData.xaml
    /// </summary>
    public partial class DetectorEfficiencyDataView : UserControl
    {
        public DetectorEfficiencyDataViewModel ViewModel { get; set; } = new DetectorEfficiencyDataViewModel();

        public DetectorEfficiencyDataView()
        {
            ViewModel.Filter.Properties = typeof(EfficiencyDataTableItem).GetProperties().ToList();
            DataContext = ViewModel;
            InitializeComponent();

            if (Dataset.DetectorEfficiencies is not null)
            {
                ViewModel.AllTableData = new();
                foreach (DetectorEfficiency detectorEfficiency in Dataset.DetectorEfficiencies)
                {
                    if (detectorEfficiency.Energies is null || detectorEfficiency.TotalAttenuations is null || detectorEfficiency.TotalPhotofractions is null) continue;
                    if (detectorEfficiency.Energies.Count == detectorEfficiency.TotalAttenuations.Count && detectorEfficiency.Energies.Count == detectorEfficiency.TotalPhotofractions.Count)
                    {
                        for (int index = 0; index < detectorEfficiency.Energies.Count; index++)
                        {
                            ViewModel.AllTableData.Add(new EfficiencyDataTableItem()
                            {
                                Detector = detectorEfficiency.Detector,
                                Window = detectorEfficiency.Window,
                                Energy = detectorEfficiency.Energies[index],
                                TotalAttenuation = detectorEfficiency.TotalAttenuations[index],
                                TotalPhotofraction = detectorEfficiency.TotalPhotofractions[index]
                            });
                        }
                    }
                }
                ViewModel.FilteredTableData = new(ViewModel.AllTableData);
                ViewModel.OnPropertyChanged();
            }
        }

        public void FilterData(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Filter.SelectedColumnName is null || ViewModel.AllTableData is null) return;
            if (ViewModel.Filter.IsRange)
            {
                ViewModel.FilteredTableData.Clear();
                ViewModel.Filter.ValueFrom = ViewModel.Filter.ValueFrom.Replace(".", ",");
                ViewModel.Filter.ValueTo = ViewModel.Filter.ValueTo.Replace(".", ",");
                if (double.TryParse(ViewModel.Filter.ValueFrom, out double temp) && double.TryParse(ViewModel.Filter.ValueTo, out temp))
                {
                    ViewModel.FilteredTableData.AddRange(ViewModel.AllTableData.Where(elementTableItem =>
                        Convert.ToDouble(elementTableItem.GetType().GetProperty(ViewModel.Filter.SelectedColumnName)?.GetValue(elementTableItem, null)) >= Convert.ToDouble(ViewModel.Filter.ValueFrom)
                        && Convert.ToDouble(elementTableItem.GetType().GetProperty(ViewModel.Filter.SelectedColumnName)?.GetValue(elementTableItem, null)) <= Convert.ToDouble(ViewModel.Filter.ValueTo)));
                }
            }
            else if (ViewModel.Filter.IsEqual)
            {
                ViewModel.FilteredTableData.Clear();
                ViewModel.FilteredTableData.AddRange(ViewModel.AllTableData.Where(elementTableItem =>
                elementTableItem.GetType().GetProperty(ViewModel.Filter.SelectedColumnName)?.GetValue(elementTableItem, null)?.ToString() == ViewModel.Filter.Value));
            }
            else if (ViewModel.Filter.IsContains)
            {
                ViewModel.FilteredTableData.Clear();
                ViewModel.FilteredTableData.AddRange(ViewModel.AllTableData.Where(elementTableItem =>
                elementTableItem.GetType().GetProperty(ViewModel.Filter.SelectedColumnName)?.GetValue(elementTableItem, null)?.ToString()?.Contains(ViewModel.Filter.Value) ?? false));
            }
            else if (ViewModel.Filter.IsStartsWith)
            {
                ViewModel.FilteredTableData.Clear();
                ViewModel.FilteredTableData.AddRange(ViewModel.AllTableData.Where(elementTableItem =>
                elementTableItem.GetType().GetProperty(ViewModel.Filter.SelectedColumnName)?.GetValue(elementTableItem, null)?.ToString()?.StartsWith(ViewModel.Filter.Value) ?? false));
            }
            else if (ViewModel.Filter.IsEndsWith)
            {
                ViewModel.FilteredTableData.Clear();
                ViewModel.FilteredTableData.AddRange(ViewModel.AllTableData.Where(elementTableItem =>
                elementTableItem.GetType().GetProperty(ViewModel.Filter.SelectedColumnName)?.GetValue(elementTableItem, null)?.ToString()?.EndsWith(ViewModel.Filter.Value) ?? false));
            }
        }

        public void ResetFilter(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AllTableData == null) return;

            ViewModel.Filter.SelectedColumnName = null;
            ViewModel.Filter.SelectedFilterOperation = null;
            ViewModel.Filter.Value = string.Empty;
            ViewModel.Filter.ValueFrom = string.Empty;
            ViewModel.Filter.ValueTo = string.Empty;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataGridControl.ItemsSource);
            view.SortDescriptions.Clear();
            foreach (DataGridColumn column in dataGridControl.Columns)
            {
                column.SortDirection = null;
            }

            ViewModel.FilteredTableData = new(ViewModel.AllTableData);
            ViewModel.OnPropertyChanged();
        }
    }
}
