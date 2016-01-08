using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AutoHome.Universal.Models
{
    [DataContract]
    public class DomoticzLightSwitch
    {
        [DataMember(Name = "IsDimmer")]
        public Boolean IsDimmer { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "SubType")]
        public string SubType { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "idx")]
        public int idx { get; set; }
    }
}
