using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.DataTable;

namespace XRayAnalyzer.MVVM.ViewModel.DataViewer
{
    public class XrayMassCoefficientDataViewModel : PropertyChangedBaseModel
    {
        public List<XrayMassCoefficientDataTableItem>? AllTableData { get; set; }

        public ObservableCollection<XrayMassCoefficientDataTableItem> FilteredTableData { get; set; } = new ObservableCollection<XrayMassCoefficientDataTableItem>();

        public FilterViewModel Filter { get; set; } = new FilterViewModel();
    }
}
