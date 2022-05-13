using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.DataTable;

namespace XRayAnalyzer.MVVM.ViewModel.DataViewer
{
    public class ElementLineDataViewModel : PropertyChangedBaseModel
    {
        public List<ElementLineDataTableItem>? AllTableData { get; set; }

        public ObservableCollection<ElementLineDataTableItem> FilteredTableData { get; set; } = new ObservableCollection<ElementLineDataTableItem>();

        public FilterViewModel Filter { get; set; } = new FilterViewModel();
    }
}
