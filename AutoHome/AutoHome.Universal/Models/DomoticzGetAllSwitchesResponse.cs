using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AutoHome.Universal.Models
{
    [DataContract]
    public class DomoticzGetAllSwitchesResponse : BaseDomoticzResponse
    {
        [DataMember(Name = "result")]
        public DomoticzLightSwitch[] LightSwitches { get; set; }

    }
}
