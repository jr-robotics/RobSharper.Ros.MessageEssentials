using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;
using Joanneum.Robotics.Ros.PackageXml.V2;
using Xunit;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests
{
    public class CodeGenerationContextTests
    {
        [Fact]
        public void Can_create_CodeGenerationContext_for_standard_package()
        {
            var context = CodeGenerationContext.Create(Path.Combine("TestPackages", "std_msgs"));

            context.Should().NotBeNull();

            context.Packages.Should().NotContainNulls();
            context.Packages.Should().ContainSingle();

            var package = context.Packages.First().PackageInfo;

            package.Name.Should().Be("std_msgs");
            package.IsMetaPackage.Should().BeFalse();
            package.PackageDirectory.FullName.Should().Be(Path.GetFullPath(Path.Combine("TestPackages", "std_msgs")));
        }
        
        [Fact]
        public void Can_create_CodeGenerationContext_for_nested_package()
        {
            var context = CodeGenerationContext.Create(Path.Combine("TestPackages", "control_msgs"));

            context.Should().NotBeNull();

            context.Packages.Should().NotContainNulls();
            context.Packages.Should().ContainSingle();

            var package = context.Packages.First().PackageInfo;
            
            package.Name.Should().Be("control_msgs");
            package.IsMetaPackage.Should().BeFalse();
            package.PackageDirectory.FullName.Should().Be(Path.GetFullPath(Path.Combine("TestPackages", "control_msgs", "control_msgs")));
        }
        
        [Fact]
        public void Can_create_CodeGenerationContext_for_meta_package()
        {
            var context = CodeGenerationContext.Create(Path.Combine("TestPackages", "common_msgs"));

            context.Should().NotBeNull();

            context.Packages.Should().NotContainNulls();
            context.Packages.Count().Should().Be(10);

            context.Packages.First(x => x.PackageInfo.Name == "common_msgs").PackageInfo.IsMetaPackage.Should().BeTrue();
        }

        [Fact]
        public void Can_reorder_packages_for_building_if_only_one_package_should_be_built()
        {
            var context = CodeGenerationContext.Create(Path.Combine("TestPackages", "std_msgs"));
            
            context.ReorderPackagesForBuilding();
            context.Packages.Count().Should().Be(1);
        }

        [Fact(Skip = "true")]
        public void Can_reorder_packages_for_building_dependent_packages()
        {
            var context = CodeGenerationContext.Create(Path.Combine("TestPackages", "common_msgs"));

            var initialNames = context.Packages.Select(x => x.PackageInfo.Name).ToList();
            
            context.ReorderPackagesForBuilding();
            context.Packages.Count().Should().Be(10);

            var finalNames = context.Packages.Select(x => x.PackageInfo.Name).ToList();

            initialNames.Should().Equals(finalNames);
            context.Packages.Last().PackageInfo.Name.Should().Be("common_msgs");
        }
    }
}