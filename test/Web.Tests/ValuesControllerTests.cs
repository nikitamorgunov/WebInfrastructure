﻿namespace Web.Tests
{
    using System.Collections.Generic;
    using Client.ServicesClients;
    using Skeleton.Web.Integration.Exceptions;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    public class ValuesControllerTests : BaseServiceClientTests<BaseApiTestsFixture<TestsStartup>, ValuesServiceClient>
    {
        public ValuesControllerTests(BaseApiTestsFixture<TestsStartup> fixture) : base(fixture)
        {
        }

        [Fact]
        public void ShouldReturnValues()
        {
            Assert.NotEmpty(ServiceClient.Get());
            Fixture.MockLogger.VerifyNoErrors();
        }

        [Fact]
        public async void ShouldReturnValuesAsync()
        {
            Assert.NotEmpty(await ServiceClient.GetAsync());
            Fixture.MockLogger.VerifyNoErrors();
        }

        [Fact]
        public void ShouldSetValue()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            ServiceClient.Set(id, expectedValue);
            var actualValue = ServiceClient.Get(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger.VerifyNoErrors();
        }

        [Fact]
        public async void ShouldSetValueAsync()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            await ServiceClient.SetAsync(id, expectedValue);
            var actualValue = await ServiceClient.GetAsync(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger.VerifyNoErrors();
        }

        [Fact]
        public void ShouldNotValidateNegativeKeys()
        {
            Assert.Throws<BadRequestException>(() => ServiceClient.Post(-1, "test"));
        }

        [Fact]
        public async void ShouldValidatePositiveKeys()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            await ServiceClient.PostAsync(id, expectedValue);
            var actualValue = await ServiceClient.GetAsync(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger.VerifyNoErrors();
        }

        [Fact]
        public void ShouldDeleteValue()
        {
            // Given
            const int id = 1;

            // When
            ServiceClient.Set(id, "test");
            ServiceClient.Delete(id);

            // Then
            Assert.Throws<ApiException>(() => ServiceClient.Get(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }

        [Fact]
        public async void ShouldDeleteValueAsync()
        {
            // Given
            const int id = 1;

            // When
            await ServiceClient.SetAsync(id, "test");
            await ServiceClient.DeleteAsync(id);

            // Then
            Assert.Throws<ApiException>(() => ServiceClient.Get(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhileGettingValueByNonexistentKey()
        {
            // Given
            const int id = 2;

            // When, Then
            Assert.Throws<ApiException>(() => ServiceClient.Get(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }

        [Fact]
        public async void ShouldThrowExceptionWhileGettingValueByNonexistentKeyAsync()
        {
            // Given
            const int id = 2;

            // When, Then
            await Assert.ThrowsAsync<ApiException>(async () => await ServiceClient.GetAsync(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }
    }
}