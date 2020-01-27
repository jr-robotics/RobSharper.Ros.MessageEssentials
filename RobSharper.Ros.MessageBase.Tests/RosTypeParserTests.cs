using System;
using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace RobSharper.Ros.MessageBase.Tests
{
    public class RosTypeParserTests
    {
        [Theory]
        [InlineData("std_msgs/Header")]
        [InlineData("Header")]
        public void Can_parse_header(string headerType)
        {
            const string HeaderTypeName = "std_msgs/Header";
            
            var type = RosType.Parse(headerType);

            type.Should().NotBeNull();
            type.IsHeaderType.Should().BeTrue();
            type.PackageName.Should().Be("std_msgs");
            type.TypeName.Should().Be("Header");
            type.ToString().Should().Be(HeaderTypeName);
            type.ToString("F").Should().Be(HeaderTypeName);
            type.ToString("T").Should().Be(HeaderTypeName);
            type.IsBuiltIn.Should().BeFalse();
            type.IsArray.Should().BeFalse();
        }

        [Fact]
        public void Header_can_be_defined_in_other_package_as_well()
        {
            var type = RosType.Parse("my_msgs/Header");

            type.Should().NotBeNull();
            type.IsHeaderType.Should().BeFalse();
            type.PackageName.Should().Be("my_msgs");
            type.TypeName.Should().Be("Header");
            type.ToString().Should().Be("my_msgs/Header");
            type.ToString("F").Should().Be("my_msgs/Header");
            type.ToString("T").Should().Be("my_msgs/Header");
            type.IsBuiltIn.Should().BeFalse();
        }

        [Fact]
        void Can_parse_packaged_type_name()
        {
            const string TypeName = "bar_msgs/FooMsg";

            var type = RosType.Parse(TypeName);

            type.Should().NotBeNull();
            type.IsArray.Should().BeFalse();
            type.IsFixedSizeArray.Should().BeFalse();
            type.IsDynamicArray.Should().BeFalse();
            type.ArraySize.Should().Be(0);
            type.PackageName.Should().Be("bar_msgs");
            type.TypeName.Should().Be("FooMsg");
            type.IsBuiltIn.Should().BeFalse();
            type.IsFullQualified.Should().BeTrue();
            type.IsHeaderType.Should().BeFalse();
            type.ToString().Should().Be(TypeName);
            type.ToString("T").Should().Be(TypeName);
            type.ToString("F").Should().Be(TypeName);
        }

        [Fact]
        void Can_parse_not_packaged_type_name()
        {
            const string TypeName = "FooMsg";

            var type = RosType.Parse(TypeName);

            type.Should().NotBeNull();
            type.IsArray.Should().BeFalse();
            type.IsFixedSizeArray.Should().BeFalse();
            type.IsDynamicArray.Should().BeFalse();
            type.ArraySize.Should().Be(0);
            type.PackageName.Should().BeNull();
            type.TypeName.Should().Be("FooMsg");
            type.IsBuiltIn.Should().BeFalse();
            type.IsFullQualified.Should().BeFalse();
            type.IsHeaderType.Should().BeFalse();
            type.ToString().Should().Be(TypeName);
            type.ToString("T").Should().Be(TypeName);
            type.ToString("F").Should().Be(TypeName);
        }

        [Theory]
        [InlineData("int32[]", false, 0)]
        [InlineData("int32[64]", true, 64)]
        [InlineData("FoooType[]", false, 0)]
        [InlineData("FooType[12]", true, 12)]
        [InlineData("bar_msgs/FooType[]", false, 0)]
        [InlineData("bar_msgs/FooType[32]", true, 32)]
        public void Can_parse_arrays(string rosType, bool isFixedSizeArray, int arraySize)
        {
            var type = RosType.Parse(rosType);

            type.Should().NotBeNull();
            type.IsArray.Should().BeTrue();
            type.IsFixedSizeArray.Should().Be(isFixedSizeArray);
            type.IsDynamicArray.Should().Be(!isFixedSizeArray);
            type.ArraySize.Should().Be(arraySize);
            type.ToString().Should().Be(rosType);
        }

        [Theory]
        [InlineData("int32[]", null, "int32")]
        [InlineData("int32[64]", null, "int32")]
        [InlineData("FooType[]", null, "FooType")]
        [InlineData("FooType[12]", null, "FooType")]
        [InlineData("bar_msgs/FooType[]", "bar_msgs", "FooType")]
        [InlineData("bar_msgs/FooType[32]", "bar_msgs", "FooType")]
        public void Array_is_striped_from_type_name(string rosType, string expectedPackageName, string expectedTypeName)
        {
            var type = RosType.Parse(rosType);

            type.Should().NotBeNull();
            type.IsArray.Should().BeTrue();
            type.PackageName.Should().Be(expectedPackageName);
            type.TypeName.Should().Be(expectedTypeName);
        }

        [Theory]
        [InlineData("int32[]", "int32")]
        [InlineData("int32[64]", "int32")]
        [InlineData("FooType[]", "FooType")]
        [InlineData("FooType[12]", "FooType")]
        [InlineData("bar_msgs/FooType[]", "bar_msgs/FooType")]
        [InlineData("bar_msgs/FooType[32]", "bar_msgs/FooType")]
        public void Tostring_returns_array_type_with_specifier(string rosType, string rosTypeWithoutArray)
        {
            var type = RosType.Parse(rosType);

            type.Should().NotBeNull();
            type.ToString().Should().Be(rosType);

            type.ToString("F").Should().Be(rosType);
            type.ToString("T").Should().Be(rosTypeWithoutArray);
        }

        [Theory]
        [InlineData("int8")]
        [InlineData("int8[]")]
        [InlineData("int16")]
        [InlineData("int16[]")]
        [InlineData("int32")]
        [InlineData("int32[]")]
        [InlineData("int64")]
        [InlineData("int64[]")]
        [InlineData("uint8")]
        [InlineData("uint8[]")]
        [InlineData("uint16")]
        [InlineData("uint16[]")]
        [InlineData("uint32")]
        [InlineData("uint32[]")]
        [InlineData("uint64")]
        [InlineData("uint64[]")]
        [InlineData("float32")]
        [InlineData("float64")]
        [InlineData("bool")]
        [InlineData("string")]
        [InlineData("time")]
        [InlineData("duration")]
        [InlineData("char")]
        [InlineData("byte")]
        public void Can_parse_built_in_types(string rosType)
        {
            var type = RosType.Parse(rosType);

            type.Should().NotBeNull();
            type.ToString().Should().Be(rosType);
            type.TypeName.Should().Be(rosType.TrimEnd('[', ']'));
            type.PackageName.Should().BeNull();
            type.IsBuiltIn.Should().BeTrue();
            type.IsFullQualified.Should().BeTrue();
        }

        [Theory]
        [InlineData("my_package//Type")]
        [InlineData("my_package/")]
        [InlineData("0Type")]
        [InlineData("Package[]/type")]
        [InlineData("Type space")]
        [InlineData("/Type")]
        [InlineData("[]")]
        [InlineData("[12]")]
        [InlineData("Package/[]")]
        [InlineData("Package/[12]")]
        public void Invalid_name_thrpws_exception(string name)
        {
            Action parse = () => RosType.Parse(name);

            parse.Should().Throw<FormatException>();
        }
    }
}