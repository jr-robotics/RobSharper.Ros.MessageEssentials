using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;
using Xunit;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests
{
    public class CodeGenerationContextTests
    {
        [Fact]
        public void Can_create_CodeGenerationContext_for_standard_package()
        {
            var context = CodeGenerationContext.Create("TestPackages\\std_msgs");

            context.Should().NotBeNull();

            context.Packages.Should().NotContainNulls();
            context.Packages.Should().ContainSingle();

            context.Packages.First().Name.Should().Equals("std_msgs");
            context.Packages.First().IsMetaPackage.Should().BeFalse();
            context.Packages.First().SourcePath.Should().Equals(Path.GetFullPath("TestPackages\\std_msgs"));
        }
        
        [Fact]
        public void Can_create_CodeGenerationContext_for_nested_package()
        {
            var context = CodeGenerationContext.Create("TestPackages\\control_msgs");

            context.Should().NotBeNull();

            context.Packages.Should().NotContainNulls();
            context.Packages.Should().ContainSingle();

            context.Packages.First().Name.Should().Equals("control_msgs");
            context.Packages.First().IsMetaPackage.Should().BeFalse();
            context.Packages.First().SourcePath.Should().Equals(Path.GetFullPath("TestPackages\\control_msgs\\control_msgs"));
        }
        
        [Fact]
        public void Can_create_CodeGenerationContext_for_meta_package()
        {
            var context = CodeGenerationContext.Create("TestPackages\\common_msgs");

            context.Should().NotBeNull();

            context.Packages.Should().NotContainNulls();
            context.Packages.Count().Should().Be(10);

            context.Packages.First(x => x.Name == "common_msgs").IsMetaPackage.Should().BeTrue();
        }
    }
}