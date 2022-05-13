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
    public class FluorescentYieldDataViewModel : PropertyChangedBaseModel
    {
        public List<FluorescentYieldDataTableItem>? AllTableData { get; set; }

        public ObservableCollection<FluorescentYieldDataTableItem> FilteredTableData { get; set; } = new ObservableCollection<FluorescentYieldDataTableItem>();

        public FilterViewModel Filter { get; set; } = new FilterViewModel();
    }
}
