using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication
{
    public interface IPropertiesBaseInterface
    {
        public string FunctionName { get; }
    }
}
