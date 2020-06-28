using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public static class SerializationManagement
{

    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
        {
            obj = new NullRepresentative();
        }
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static object ByteArrayToObject(byte[] obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(obj, 0, obj.Length);
            ms.Seek(0, SeekOrigin.Begin);
            object toReturn = bf.Deserialize(ms);
            if (toReturn.GetType() == typeof(NullRepresentative))
            {
                return null;
            }
            else { return toReturn; }
        }
    }

    public static T ByteArrayToObject<T>(byte[] obj)
    {
        return (T)ByteArrayToObject(obj);
    }

    public static string ObjectToXML(object dataToSerialize)
    {
        using (StringWriter stringwriter = new StringWriter())
        {
            var serializer = new XmlSerializer(dataToSerialize.GetType());
            serializer.Serialize(stringwriter, dataToSerialize);
            return stringwriter.ToString();
        }
    }

    public static T XMLToObject<T>(string xmlText)
    {
        using (StringReader stringReader = new StringReader(xmlText))
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(stringReader);
        }
    }
}

[Serializable]
public class NullRepresentative { }