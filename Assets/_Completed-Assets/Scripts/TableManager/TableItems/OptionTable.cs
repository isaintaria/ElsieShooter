using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets._Completed_Assets.Scripts.TableManager.TableItems
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    [Serializable]
    public class EffectOption
    {
        
      //  [XmlAttribute("Vibration")]
        public bool vibration;
      //  [XmlAttribute("Bulb")]
        public bool bulb;
      //  [XmlAttribute("Speaker")]
        public bool speaker;
      //  [XmlAttribute("Pattern_Vibration")]
        public int pattern_v;
      //  [XmlAttribute("Pattern_Bulb")]
        public int pattern_b;
       
    }
    
    
    [XmlRoot("OptionTable")] 
    public class OptionTable
    {        
        
        
        public EffectOption FireBeam { get; set; }
        
        public EffectOption FireBomb { get; set; }
       
        public EffectOption Explosion_Bomb { get; set; }
        
        public EffectOption Destroy_Enemy { get; set; }
        
        public EffectOption Destroy_Player { get; set; }
        
        public EffectOption Destroy_Astroid { get; set; }
        
        public EffectOption GetBomb { get; set; }
        
        public EffectOption GetBonusScore { get; set; }
      
        public EffectOption BeamCollision { get; set; }

        public OptionTable()
        {
            FireBeam = new EffectOption();
            FireBomb = new EffectOption();
            Explosion_Bomb = new EffectOption();
            Destroy_Enemy = new EffectOption();
            Destroy_Player = new EffectOption();
            Destroy_Astroid = new EffectOption();
            GetBomb = new EffectOption();
            GetBonusScore = new EffectOption();
            BeamCollision = new EffectOption();
        }
    }
}
