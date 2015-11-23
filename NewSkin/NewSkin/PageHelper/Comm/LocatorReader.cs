using System;
using System.Xml;

namespace NewSkin.PageHelper.Comm
{
    public class LocatorReader
    { 
        public string fileName;

        public LocatorReader(string filename)
        {
            fileName = filename;
        }

        public string ReadLocator(string key)
        {
            var xmlDocument = new XmlDocument();

            xmlDocument.Load("../../Locators/" + fileName);

            var node = xmlDocument.DocumentElement.SelectSingleNode(@key);
            var childNode = node.ChildNodes[0];

            if (!(childNode is XmlCDataSection)) return "";
            return (childNode as XmlCDataSection).Value;
        }
 
    }
}