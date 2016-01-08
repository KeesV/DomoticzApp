using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AutoHome.Universal.Models
{
    public enum DomoticzResponseStatus
    {
        OK,
        ERR
    }

    [DataContract]
    public class BaseDomoticzResponse
    {
        [DataMember(Name = "status")]
        public DomoticzResponseStatus Status { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
