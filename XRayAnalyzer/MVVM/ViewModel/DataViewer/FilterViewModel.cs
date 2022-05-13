using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.DataViewer
{
    public class FilterViewModel : PropertyChangedBaseModel
    {
        public List<PropertyInfo>? Properties { get; set; }

        public List<string>? ColumnNames
        {
            get
            {
                return Properties?.Select(propertyInfo => propertyInfo.Name).ToList();
            }
        }

        public string? selectedColumnName;
        public string? SelectedColumnName
        {
            get
            {
                return selectedColumnName;
            }
            set
            {
                Type? type = Properties?.FirstOrDefault(propertyInfo => propertyInfo.Name == value)?.PropertyType;
                IsString = type == typeof(string);
                IsNumber = type == typeof(int) || type == typeof(double) || type == typeof(float) || type == typeof(decimal);
                SelectedFilterOperation = null;
                if (IsString == false && IsNumber == false)
                {
                    selectedColumnName = null;
                    OnPropertyChanged(nameof(SelectedColumnName));
                    return;
                }

                selectedColumnName = value;
                OnPropertyChanged(nameof(SelectedColumnName));
            }
        }

        public string? selectedFilterOperation;
        public string? SelectedFilterOperation
        {
            get
            {
                return selectedFilterOperation;
            }
            set
            {
                selectedFilterOperation = value;
                OnPropertyChanged(nameof(SelectedFilterOperation));
            }
        }

        public string value = string.Empty;
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public string valueFrom = string.Empty;
        public string ValueFrom
        {
            get
            {
                return valueFrom;
            }
            set
            {
                valueFrom = value;
                OnPropertyChanged(nameof(ValueFrom));
            }
        }

        public string valueTo = string.Empty;
        public string ValueTo
        {
            get
            {
                return valueTo;
            }
            set
            {
                valueTo = value;
                OnPropertyChanged(nameof(ValueTo));
            }
        }

        public bool isNumber = false;
        public bool IsNumber
        {
            get
            {
                return isNumber;
            }
            set
            {
                isNumber = value;
                OnPropertyChanged(nameof(IsNumber));
            }
        }

        public bool isString = false;
        public bool IsString
        {
            get
            {
                return isString;
            }
            set
            {
                isString = value;
                OnPropertyChanged(nameof(IsString));
            }
        }

        public bool isEqual = false;
        public bool IsEqual
        {
            get
            {
                return isEqual;
            }
            set
            {
                isEqual = value;
                OnPropertyChanged(nameof(IsEqual));
            }
        }

        public bool isRange = false;
        public bool IsRange
        {
            get
            {
                return isRange;
            }
            set
            {
                isRange = value;
                OnPropertyChanged();
            }
        }

        public bool isContains = false;
        public bool IsContains
        {
            get
            {
                return isContains;
            }
            set
            {
                isContains = value;
                OnPropertyChanged(nameof(IsContains));
            }
        }

        public bool isStartsWith = false;
        public bool IsStartsWith
        {
            get
            {
                return isStartsWith;
            }
            set
            {
                isStartsWith = value;
                OnPropertyChanged(nameof(IsStartsWith));
            }
        }

        public bool isEndsWith = false;
        public bool IsEndsWith
        {
            get
            {
                return isEndsWith;
            }
            set
            {
                isEndsWith = value;
                OnPropertyChanged(nameof(IsEndsWith));
            }
        }

        public bool IsSingleValue
        {
            get
            {
                return !IsRange;
            }
        }

        public bool IsRangeValue
        {
            get
            {
                return IsRange;
            }
        }
    }
}
