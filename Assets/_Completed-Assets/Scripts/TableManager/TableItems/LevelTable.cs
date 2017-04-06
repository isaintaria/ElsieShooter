using System.Collections.Generic;
using System.Xml.Serialization;

public class LevelProperty
{
    public LevelProperty()
    {

    }
    public LevelProperty(int level, int timeline, int ast1, int ast2, int ast3, int normal, int green, int red, int speed)
    {
        this.level = level;
        this.timeline = timeline;
        this.ast1 = ast1;
        this.ast2 = ast2;
        this.ast3 = ast3;
        this.normal = normal;
        this.green = green;
        this.red = red;
        this.speed = speed;
    }

    [XmlAttribute("Level")]
     public int level;
    [XmlAttribute("Timeline")]
     public int timeline;
    [XmlAttribute("Ast_1")]
     public int ast1;
    [XmlAttribute("Ast_2")]
     public int ast2;
    [XmlAttribute("Ast_3")]
     public int ast3;
    [XmlAttribute("Normal")]
     public int normal;
    [XmlAttribute("Green")]
     public int green;
    [XmlAttribute("Red")]
     public int red;
    [XmlAttribute("Speed")]
     public float speed;
}

[XmlRoot("LevelTable")]
public class LevelTable
{
    private static List<LevelProperty> s_datas = new List<LevelProperty>();

    [XmlArray("Levels")]
    [XmlArrayItem("Level")]
    public LevelProperty[] Datas
    {
        get { return s_datas.ToArray(); }
        set { s_datas = new List<LevelProperty>(value); }
    }

    public static LevelProperty GetProperty(int level)
    {
        LevelProperty property = s_datas.Find(rhs => rhs.level == level);

        if (null == property)
        {
            UnityEngine.Debug.Log("찾으려는 Data가 존재하지 않습니다.");
            return null;
        }

        return property;
    }
}