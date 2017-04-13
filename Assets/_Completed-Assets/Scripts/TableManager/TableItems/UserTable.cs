using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Assets._Completed_Assets.Scripts.TableManager.TableItems
{


    [XmlRoot("UserTable")]
    public class UserTable
    {
        [XmlIgnore]
        public  List<UserProperty> s_datas = new List<UserProperty>();

        [XmlArray("Users")]
        [XmlArrayItem("User")]
        public UserProperty[] Datas
        {
            get { return s_datas.ToArray(); }
            set { s_datas = new List<UserProperty>(value); }
        }
                
    }
    [Serializable]
    public class UserProperty
    {
        public string Id { get; set; }
        public int Score { get; set; }
        public bool Is3DMode { get; set; }
        public bool IsVisualEffectOn { get; set; }
        public bool IsHapticEffectOn { get; set; }
    }
}
