using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Assets._Completed_Assets.Scripts.TableManager.TableItems
{
    [Serializable]
    public class PortInfo
    {
        public string PORT { get; set;}
        public PortInfo()
        {

        }
    }
    [XmlRoot("PortTable")]
    public class PortTable
    {
        public PortTable()
        {
            Bulb_Port = new PortInfo();
            Vibe_Port = new PortInfo();
        }

        public PortInfo Bulb_Port { get; set; }
        public PortInfo Vibe_Port { get; set; }

      
    }
}
