using System.Xml.Linq;

namespace NewSkin.Util
{
    internal class XmlReader
    {
        public static dynamic Read(string file)
        {
            var path = "../../" + file;
            return XDocument.Load(path).Root.ToDynamic();
        }
    }
}