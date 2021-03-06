﻿namespace Skeleton.Common.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using JetBrains.Annotations;
    using Xunit;

    public class EnumerableExtensionsTests
    {
        [UsedImplicitly]
        public static IEnumerable<object[]> IsEmptyExtensionTests;
        [UsedImplicitly]
        public static IEnumerable<object[]> IsNotEmptyExtensionTests;
        [UsedImplicitly]
        public static IEnumerable<object[]> AsArrayExtensionTests;
        [UsedImplicitly]
        public static IEnumerable<object[]> IsEqualsExtensionTests;

        static EnumerableExtensionsTests()
        {
            IsEmptyExtensionTests = new[]
                                    {
                                        new object[] {null, true},
                                        new object[] {Enumerable.Empty<int>(), true},
                                        new object[] {Enumerable.Repeat(1, 2), false}
                                    };
            IsNotEmptyExtensionTests = new[]
                                       {
                                           new object[] {null, false},
                                           new object[] {Enumerable.Empty<int>(), false},
                                           new object[] {Enumerable.Repeat(1, 2), true}
                                       };
            var arrayForTesting = new[] {1, 2, 3, 4, 5};
            AsArrayExtensionTests = new[]
                                    {
                                        new object[] {null, new int[0]},
                                        new object[] {Enumerable.Range(1, 3), new[] {1, 2, 3}},
                                        new object[] {arrayForTesting, arrayForTesting}
                                    };
            IsEqualsExtensionTests = new[]
                                     {
                                         new object[] {null, null, true},
                                         new object[] {Enumerable.Empty<int>(), Enumerable.Empty<int>(), true},
                                         new object[] {null, Enumerable.Empty<int>(), false},
                                         new object[] {Enumerable.Empty<int>(), null, false},
                                         new object[] {Enumerable.Empty<int>(), Enumerable.Repeat(1, 2), false},
                                         new object[] {Enumerable.Repeat(1, 2), Enumerable.Repeat(1, 2), true}
                                     };
        }

        [Theory]
        [MemberData(nameof(IsEmptyExtensionTests))]
        public void ShouldCheckCollectionsForEmptiness(IEnumerable<int> source, bool expected)
        {
            Assert.Equal(expected, source.IsEmpty());
        }

        [Theory]
        [MemberData(nameof(IsNotEmptyExtensionTests))]
        public void ShouldCheckCollectionsForNotEmptiness(IEnumerable<int> source, bool expected)
        {
            Assert.Equal(expected, source.IsNotEmpty());
        }

        [Theory]
        [MemberData(nameof(AsArrayExtensionTests))]
        public void ShouldCheckEnumerableToArrayConversion(IEnumerable<int> source, int[] expected)
        {
            Assert.Equal(expected, source.AsArray());
        }

        [Theory]
        [MemberData(nameof(IsEqualsExtensionTests))]
        public void ShouldCheckCollectionsForEquals(IEnumerable<int> first, IEnumerable<int> second, bool expected)
        {
            Assert.Equal(expected, first.IsEquals(second));
        }
    }
}