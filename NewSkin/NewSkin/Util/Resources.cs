using System.IO;

namespace NewSkin.Util
{
    internal class Resources
    {
        /// <summary>
        ///     Get the abosolute path of a file in the Resources/ directory.
        /// </summary>
        /// <param name="file">The name of the file in Resources/</param>
        /// <returns>The absolute directory of that file.</returns>
        public static string GetPath(string file)
        {
            return Path.GetFullPath("../../Resources/" + file);
        }
    }
}