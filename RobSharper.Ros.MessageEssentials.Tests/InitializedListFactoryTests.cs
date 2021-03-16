using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class InitializedListFactoryTests
    {
        [Theory]
        [InlineData(typeof(bool))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(short))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(TimeSpan))]
        [InlineData(typeof(RosTime))]
        [InlineData(typeof(RosDuration))]
        [InlineData(typeof(object))]
        public void CanInitializeList(Type listType)
        {
            const int size = 5;

            var list = InitializedListFactory.Create(listType, size);
            
            list.Should().NotBeNull();
            list.Should().HaveCount(size);
            list.Should().NotContainNulls();
        }
        
        [Theory]
        [InlineData(typeof(bool))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(short))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(TimeSpan))]
        [InlineData(typeof(RosTime))]
        [InlineData(typeof(RosDuration))]
        [InlineData(typeof(object))]
        public void CanInitializeGenericList(Type elementType)
        {
            const int size = 5;

            var createMethod = typeof(InitializedListFactory)
                .GetMethod("Create", new [] {typeof(int)})
                .MakeGenericMethod(elementType);

            var list = (IList) createMethod.Invoke(null, new object[] {size});

            list.Should().NotBeNull();
            list.Should().HaveCount(size);
            list.Should().NotContainNulls();
        }
        
        [Fact]
        public void CanInitializeListWithSizeZero()
        {
            const int size = 0;

            var list = InitializedListFactory.Create<int>(size);
            
            list.Should().NotBeNull();
            list.Should().HaveCount(size);
        }
        
        [Fact]
        public void CanInitializeListWithNegativeSize()
        {
            Action initAction = () => InitializedListFactory.Create<int>(-5);
            initAction.Should().Throw<ArgumentOutOfRangeException>();
        }
        

        [Fact]
        public void InitializingListForClassWithoutDefaultConstrcutorThrowsException()
        {
            Action initAction = () => InitializedListFactory.Create<ClassWithoutDefaultConstructor>(5);
            initAction.Should().Throw<MissingMethodException>();
        }
        
        public class ClassWithoutDefaultConstructor
        {
            public ClassWithoutDefaultConstructor(int value)
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                
                Value = value;
            }

            public int Value { get; set; }
        }

        [Theory]
        [InlineData(typeof(IList<int>), typeof(int))]
        [InlineData(typeof(ICollection<int>), typeof(int))]
        [InlineData(typeof(IEnumerable<int>), typeof(int))]
        [InlineData(typeof(List<int>), typeof(int))]
        public void CanUnwrapListType(Type listTye, Type expectedElementType)
        {
            var underlyingType = InitializedListFactory.UnwrapListType(listTye);

            underlyingType.Should().NotBeNull();
            underlyingType.Should().Be(expectedElementType);
        }
    }
}