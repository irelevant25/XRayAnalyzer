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
    public class DetectorEfficiencyDataViewModel : PropertyChangedBaseModel
    {
        public List<EfficiencyDataTableItem>? AllTableData { get; set; }

        public ObservableCollection<EfficiencyDataTableItem> FilteredTableData { get; set; } = new ObservableCollection<EfficiencyDataTableItem>();

        public FilterViewModel Filter { get; set; } = new FilterViewModel();
    }
}
