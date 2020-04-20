using GiG.Core.Data.Tests.Unit.Mocks;
using Xunit;

namespace GiG.Core.Data.Serializers.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class JsonDataSerializerTests
    {
        private readonly JsonDataSerializer<MockCountry> _sut;

        public JsonDataSerializerTests()
        {
            _sut = new JsonDataSerializer<MockCountry>();
        }

        [Fact]
        public void JsonDataSerializer_DeserializeJsonData_Success()
        {
            // Arrange
            var data = @"{""Alpha2Code"":""mt""}";
            var mockCountry = new MockCountry() { Alpha2Code = "mt" };

            // Act
            var country = _sut.Deserialize(data);

            // Assert
            Assert.NotNull(country);
            Assert.Equal(mockCountry.Alpha2Code, country.Alpha2Code);
        }

        [Fact]
        public void JsonDataSerializer_SerializeJsonData_Success()
        {
            // Arrange
            var data = @"{""Alpha2Code"":""mt""}";
            var mockCountry = new MockCountry() { Alpha2Code = "mt" };

            // Act
            var value = _sut.Serialize(mockCountry);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(data, value);
        }
    }
}