using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using FluentAssertions;
using Xunit;

namespace Joanneum.Robotics.Ros.PackageXml.Tests
{
    public class PackageXmlDeserializationTests
    {
        [Theory]
        [InlineData("PackageXmlFiles/control_msgs.package.xml", "control_msgs", "1.5.0")]
        public void CanDeserializePackageXmlFile(string filename, string expePackageName, string expectedVersion)
        {
            var serializer = new XmlSerializer(typeof(PackageXml.V2.package));
            var package = (PackageXml.V2.package)serializer.Deserialize(new XmlTextReader(filename));

            package.Should().NotBeNull();
            package.name.Should().Equals(expePackageName);
            package.version.Should().Equals(expectedVersion);
        }
        
        [Theory]
        [InlineData("PackageXmlFiles/depend.package.xml", "buildtool_package_dependency")]
        [InlineData("PackageXmlFiles/depend.package.xml", "build_package_dependency")]
        [InlineData("PackageXmlFiles/depend.package.xml", "run_package_dependency")]
        public void Parsed_package_xml_contains_package_dependencies(string filename, string expectedDependency)
        {
            var serializer = new XmlSerializer(typeof(PackageXml.V2.package));
            var package = (PackageXml.V2.package)serializer.Deserialize(new XmlTextReader(filename));

            package.Should().NotBeNull();
            
            package.Items.Should().NotBeNull();
            package.Items.Should().NotBeEmpty();

            package.Items.Select(x => x.Value).Should().Contain(expectedDependency);
        }
    }
}