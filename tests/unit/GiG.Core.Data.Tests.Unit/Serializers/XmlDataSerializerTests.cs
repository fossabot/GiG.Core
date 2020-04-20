using GiG.Core.Data.Tests.Unit.Helpers;
using GiG.Core.Data.Tests.Unit.Mocks;
using Xunit;

namespace GiG.Core.Data.Serializers.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class XmlDataSerializerTests
    {
        private readonly XmlDataSerializer<MockCountry> _sut;

        public XmlDataSerializerTests()
        {
            _sut = new XmlDataSerializer<MockCountry>();
        }

        [Fact]
        public void XmlDataSerializer_DeserializeXmlData_Success()
        {
            // Arrange
            var data =
                @"<?xml version=""1.0"" encoding=""utf-16""?><MockCountry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><Alpha2Code>mt</Alpha2Code></MockCountry>";

            var mockCountry = new MockCountry() { Alpha2Code = "mt" };

            // Act
            var country = _sut.Deserialize(data);

            // Assert
            Assert.NotNull(country);
            Assert.Equal(mockCountry.Alpha2Code, country.Alpha2Code);
        }

        [Fact]
        public void XmlDataSerializer_SerializeXmlData_Success()
        {
            // Arrange
            var data =
                @"<?xml version=""1.0"" encoding=""utf-16""?><MockCountry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><Alpha2Code>mt</Alpha2Code></MockCountry>";

            var mockCountry = new MockCountry() { Alpha2Code = "mt" };

            // Act
            var value = _sut.Serialize(mockCountry);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(data, value.Replace("\r", "").Replace("\n", ""));
        }

        [Fact]
        public void XmlDataSerializer_SerializeXmlDataWithNamespaceManager_Success()
        {
            // Arrange
            var data =
                @"<?xml version=""1.0"" encoding=""utf-16""?><MockCountry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><Alpha2Code>mt</Alpha2Code></MockCountry>";

            var mockCountry = new MockCountry() { Alpha2Code = "mt" };

            var xmlNamespaceManager = XmlHelper.GetXmlNamespaceManager<MockCountry>();
            
            // Act
            var value = _sut.Serialize(mockCountry, xmlNamespaceManager);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(data, value.Replace("\r", "").Replace("\n", ""));
        }
    }
}