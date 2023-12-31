using System.Xml.Serialization;

namespace RestoreMonarchy.MoreHomes.Models
{
    public class VIPPermission
    {
        public VIPPermission() { }
        public VIPPermission(string tag, int value)
        {
            PermissionTag = tag;
            Value = value;
        }
        
        [XmlAttribute]
        public string PermissionTag { get; set; }
        [XmlAttribute]
        public int Value { get; set; }
    }
}
