using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xunit.gui.wpf
{
    public static class ExtensionMethods
    {

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string ToXml(this object obj)
        {
            var xser = new XmlSerializer(obj.GetType());
            string text;

            using (var memStream = new MemoryStream())
            {
                xser.Serialize(memStream, obj);
                memStream.Position = 0;
                using (TextReader txtReader = new StreamReader(memStream))
                {
                    text = txtReader.ReadToEnd();
                }
            }

            return text;
        }

        /// <summary>
        /// Persists to XML file.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="filename">The filename.</param>
        public static void PersistToXmlFile(this object obj, string filename)
        {
             File.WriteAllText(filename, obj.ToXml());
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <typeparam name="TReturnType">The type of the return type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static TReturnType LoadFromXml<TReturnType>(this TReturnType type, string filename) where TReturnType : class
        {
            var xser = new XmlSerializer(typeof (TReturnType));
            return xser.Deserialize(File.OpenText(filename)) as TReturnType;
        }
    }
}
