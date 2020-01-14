using System;
using System.Xml;
using System.Xml.Serialization;

namespace Joanneum.Robotics.Ros.PackageXml
{
    public static class PackageXmlReader
    {
        public static int GetFormatVersion(string packageXmlFilePath)
        {
            if (packageXmlFilePath == null) throw new ArgumentNullException(nameof(packageXmlFilePath));
            var reader = new XmlTextReader(packageXmlFilePath);

            return GetFormatVersion(reader);
        }
        
        public static int GetFormatVersion(XmlTextReader packageXml)
        {
            var version = -1;
            while (packageXml.Read())
            {
                if (packageXml.Name.Equals("package", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (packageXml.AttributeCount == 0)
                    {
                        version = 1;
                    }
                    else
                    {
                        var versionAttribute = packageXml.GetAttribute("format");
                        int.TryParse(versionAttribute, out version);
                    }
                    break;
                }
            }

            return version;
        }

        public static V1.package ReadV1PackageXml(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            var xmlReader = new XmlTextReader(filename);
            
            return ReadV1PackageXml(xmlReader);
        }

        private static V1.package ReadV1PackageXml(XmlTextReader xmlReader)
        {
            var serializer = new XmlSerializer(typeof(V2.package));
            var package = (V1.package) serializer.Deserialize(xmlReader);

            return package;
        }

        public static V2.package ReadV2PackageXml(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            var xmlReader = new XmlTextReader(filename);
            
            return ReadV2PackageXml(xmlReader);
        }

        private static V2.package ReadV2PackageXml(XmlTextReader xmlReader)
        {
            var serializer = new XmlSerializer(typeof(V2.package));
            var package = (V2.package) serializer.Deserialize(xmlReader);

            return package;
        }

        public static V3.package ReadV3PackageXml(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            var xmlReader = new XmlTextReader(filename);
            
            return ReadV3PackageXml(xmlReader);
        }

        private static V3.package ReadV3PackageXml(XmlTextReader xmlReader)
        {
            var serializer = new XmlSerializer(typeof(V3.package));
            var package = (V3.package) serializer.Deserialize(xmlReader);

            return package;
        }
    }
}