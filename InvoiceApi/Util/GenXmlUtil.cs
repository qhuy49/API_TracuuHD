using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace InvoiceApi.Util
{
    public static class GenXmlUtil
    {
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
        public static T Deserialize<T>(XDocument doc)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = doc.Root.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        public static XDocument Serialize(this object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());

            XDocument doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, obj);
            }

            return doc;
        }
        public static string SerializeXML<T>(this T value)
        {
            string xml = string.Empty;
            try
            {
                if (value == null)
                {
                    return string.Empty;
                }

                XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.OmitXmlDeclaration = true;

                using (StringWriter sww = new Utf8StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww, writerSettings))
                    {
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                        //Add an empty namespace and empty value
                        ns.Add("", "");

                        xsSubmit.Serialize(writer, value, ns);
                        xml = sww.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return xml;
        }

        public static T XmlToClass<T>(string xml)
        {
            try
            {

                using (var stringReader = new System.IO.StringReader(xml))
                {
                    //XmlRootAttribute xRoot = new XmlRootAttribute();
                    //xRoot.ElementName = "TTChung";
                    //// xRoot.Namespace = "http://minvoice.vn";
                    //xRoot.IsNullable = true;

                    //var serializer = new XmlSerializer(typeof(T), xRoot);
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
