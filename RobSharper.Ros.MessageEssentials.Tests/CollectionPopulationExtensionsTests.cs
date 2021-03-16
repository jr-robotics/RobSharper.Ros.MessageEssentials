using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class CollectionPopulationExtensionsTests
    {
        [Theory]
        [InlineData(typeof(List<bool>))]
        [InlineData(typeof(List<sbyte>))]
        [InlineData(typeof(List<byte>))]
        [InlineData(typeof(List<short>))]
        [InlineData(typeof(List<ushort>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(List<uint>))]
        [InlineData(typeof(List<long>))]
        [InlineData(typeof(List<ulong>))]
        [InlineData(typeof(List<float>))]
        [InlineData(typeof(List<double>))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<DateTime>))]
        [InlineData(typeof(List<TimeSpan>))]
        [InlineData(typeof(List<RosTime>))]
        [InlineData(typeof(List<RosDuration>))]
        [InlineData(typeof(List<object>))]
        public void CanInitializeList(Type listType)
        {
            const int size = 5;

            var list = (IEnumerable) Activator.CreateInstance(listType);

            var populateMethod = typeof(CollectionPopulationExtensions)
                .GetMethod(nameof(CollectionPopulationExtensions.PopulateWithInitializedRosValues))
                .MakeGenericMethod(listType.GenericTypeArguments.First());

            populateMethod.Invoke(null, new object[] {list, size});

            list.Should().NotBeNull();
            list.Should().HaveCount(size);
            list.Should().NotContainNulls();
        }
        
        [Fact]
        public void CanPopulateListWithSizeZero()
        {
            const int size = 0;

            var list = new List<int>();
            list.PopulateWithInitializedRosValues(size);
            
            list.Should().NotBeNull();
            list.Should().HaveCount(size);
        }
        
        [Fact]
        public void CanPopulateListWithNegativeSize()
        {
            var list = new List<int>();
            
            Action initAction = () => list.PopulateWithInitializedRosValues(-5);
            initAction.Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Fact]
        public void PopulateForDateTimeContainsRosTimeEpochStart()
        {
            const int size = 1;

            var list = new List<DateTime>();
            list.PopulateWithInitializedRosValues(size);

            var value = list.First();

            value.Should().Be(RosTime.EpochStart);
        }
        
        [Fact]
        public void PopulateForStringContainsEmptyString()
        {
            const int size = 1;

            var list = new List<string>();
            list.PopulateWithInitializedRosValues(size);

            var value = list.First();

            value.Should().BeEmpty();
        }
        

        [Fact]
        public void PopulateListForClassWithoutDefaultConstructorThrowsException()
        {
            var list = new List<ClassWithoutDefaultConstructor>();
            
            Action initAction = () => list.PopulateWithInitializedRosValues(5);
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

        public class UsageScenario
        {
            public IList<int> Values = new List<int>();

            public UsageScenario()
            {
                Values.PopulateWithInitializedRosValues(36);
            }
        }
    }
}