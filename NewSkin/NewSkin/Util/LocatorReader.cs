using System.Xml.Linq;
using OpenQA.Selenium;

namespace NewSkin.Util
{
    public class LocatorReader
    {
        private readonly dynamic _xml;

        /// <summary>
        ///     LocatorReader is an utility class to read xml locators in the Locators/
        ///     directory.
        /// </summary>
        /// <param name="file">The file to read relative to Locators/</param>
        public LocatorReader(string file)
        {
            _xml = XmlReader.Read("Locators/" + file);
        }

        /// <summary>
        ///     Get the Selenium By instance from the locator file.
        /// </summary>
        /// <param name="key">
        ///     The XML key to get. Nested elements are delimited by
        ///     a '.'
        /// </param>
        /// <param name="values">Values to interpolate into the XML string value.</param>
        /// <returns>The Selenium By selector object.</returns>
        public By Get(string key, params object[] values)
        {
            var locator = GetNode(key);

            if (locator == null) throw new NoSuchElementException("Locator does not exist.");

            var xpath = values.Length > 0
                ? string.Format(locator.Value, values)
                : locator.Value;

            if (locator.Attribute("type") == null) return By.XPath(xpath);

            switch (locator.Attribute("type").Value.ToLower())
            {
                case "id":
                    return By.Id(xpath);
                case "class":
                    return By.ClassName(xpath);
                default:
                    return By.XPath(xpath);
            }
        }

        private XElement GetNode(string key)
        {
            dynamic value = _xml;

            foreach (var k in key.Split('.'))
            {
                value = value[k];
            }

            return value;
        }
    }
}