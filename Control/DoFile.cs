using System.Xml;
using System.Xml.Serialization;

namespace Bot_CoursePaper.Control;

public static class DoFile
{
    public static T Deserialize<T>(string path)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        
        XmlReader textReader = XmlReader.Create(path);
        return (T)xmlSerializer.Deserialize(textReader);
    }

    public static void Serialize<T>(T actions, string path) 
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        StreamWriter write = new StreamWriter(path);
        xmlSerializer.Serialize(write, actions);
    }

}