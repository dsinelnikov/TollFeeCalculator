using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TollFeeCalculator.App.Data
{
    public class VehicleTrafficData
    {
        public int VehicleCode { get; set; }
        public string Name { get; set; }

        [XmlArrayItem("Date")]
        public List<DateTime> Dates { get; set; }
    }
}
