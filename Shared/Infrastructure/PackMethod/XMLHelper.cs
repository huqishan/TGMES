using System.Xml.Serialization;

namespace Shared.Infrastructure.PackMethod
{
    public static class XMLHelper
    {
        public static string ObjectToXML<T>(T obj)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                //序列化对象
                xml.Serialize(Stream, obj);
                using (StreamReader sr = new StreamReader(Stream))
                {
                    Stream.Position = 0;
                    string str = sr.ReadToEnd();
                    return str;
                }
            }
        }
        public static T? XMLToObject<T>(this string xml) where T : class
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(T));
                return xmldes.Deserialize(sr) as T;
            }
        }
        public static bool SaveXML<T>(T obj, string filePath)
        {
            if (string.IsNullOrEmpty(Path.GetFileName(filePath)))
            {
                filePath += $"\\{typeof(T).Name}.xml";
            }
            string? path=Path.GetDirectoryName(filePath);
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (Stream writer = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(writer, obj);
            }
            return true;
        }
        public static T? ReadXML<T>(string filePath) where T : class
        {
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (Stream read = new FileStream(filePath, FileMode.Open))
                {
                    return serializer.Deserialize(read) as T;
                }
            }
            return null;
            
        }
    }
}
