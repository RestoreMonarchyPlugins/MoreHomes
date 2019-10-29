using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestoreMonarchy.MoreHomes.Utilities
{
    public class VIPPermission
    {
        public VIPPermission() { }
        public VIPPermission(string tag, float value)
        {
            PermissionTag = tag;
            Value = value;
        }
        
        [XmlAttribute]
        public string PermissionTag { get; set; }
        [XmlAttribute]
        public float Value { get; set; }
    }
}
