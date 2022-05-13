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
    public class JumpRatioDataViewModel : PropertyChangedBaseModel
    {
        public List<JumpRatioDataTableItem>? AllTableData { get; set; }

        public ObservableCollection<JumpRatioDataTableItem> FilteredTableData { get; set; } = new ObservableCollection<JumpRatioDataTableItem>();

        public FilterViewModel Filter { get; set; } = new FilterViewModel();
    }
}
