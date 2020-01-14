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
        [InlineData("PackageXmlFiles/v1.package.xml", 1)]
        [InlineData("PackageXmlFiles/v2.package.xml", 2)]
        public void Can_determin_package_xml_version(string filename, int expectedPackageXmlVersion)
        {
            var version = PackageXmlReader.GetFormatVersion(filename);

            switch (version)
            {
                case 1:
                    var v1Package = PackageXmlReader.ReadV1PackageXml(filename);
                    break;
                case 2:
                    var v2Package = PackageXmlReader.ReadV2PackageXml(filename);
                    break;
                case 3:
                    var v3Package = PackageXmlReader.ReadV3PackageXml(filename);
                    break;
                default:
                    throw new NotSupportedException();
            }


            version.Should().Be(expectedPackageXmlVersion);
        }
        
        [Theory]
        [InlineData("PackageXmlFiles/control_msgs.package.xml", "control_msgs", "1.5.0")]
        public void CanDeserializeV1PackageXmlFile(string filename, string expePackageName, string expectedVersion)
        {
            var package = PackageXmlReader.ReadV1PackageXml(filename);

            package.Should().NotBeNull();
            package.name.Should().Equals(expePackageName);
            package.version.Should().Equals(expectedVersion);
        }
        
        [Theory]
        [InlineData("PackageXmlFiles/depend.package.xml", "buildtool_package_dependency")]
        [InlineData("PackageXmlFiles/depend.package.xml", "build_package_dependency")]
        [InlineData("PackageXmlFiles/depend.package.xml", "run_package_dependency")]
        public void Parsed_v1_package_xml_contains_package_dependencies(string filename, string expectedDependency)
        {
            var package = PackageXmlReader.ReadV1PackageXml(filename);

            package.Should().NotBeNull();
            
            package.Items.Should().NotBeNull();
            package.Items.Should().NotBeEmpty();

            package.Items.Select(x => x.Value).Should().Contain(expectedDependency);
        }
    }
}