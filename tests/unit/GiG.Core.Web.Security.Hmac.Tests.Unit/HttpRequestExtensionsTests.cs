using GiG.Core.Web.Security.Hmac.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Security.Hmac.Tests.Unit
{
    [Trait("Category","Unit")]
    public class HttpRequestExtensionsTests
    {
        private readonly string _nonceHeader;
        private readonly string _nonceValue;

        public HttpRequestExtensionsTests()
        {
            _nonceHeader = "X-Nonce";
            _nonceValue = "abc";
        }
        [Fact]
        public async Task GetBody_Get_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context)
            {
                Method = "GET",
                Path = new PathString("/api/test")
            };
            request.Headers.Add(_nonceHeader, _nonceValue);

            //Act
            var result = await request.GetBodyAsync();

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task GetBody_Post_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context)
            {
                Method = "POST",
                Path = new PathString("/api/test")
            };
            request.Headers.Add(_nonceHeader, _nonceValue);
            var body = "woop woop I unit tested the body woop woop";
            request.Body = new MemoryStream();
            using StreamWriter bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.GetBodyAsync();

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal(body, result);
        }

        [Fact]
        public async Task GetBody_Patch_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context)
            {
                Method = "PATCH",
                Path = new PathString("/api/test")
            };
            request.Headers.Add(_nonceHeader, _nonceValue);
            var body = "woop woop I unit tested the body woop woop";
            request.Body = new MemoryStream();
            using StreamWriter bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.GetBodyAsync();

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal(body, result);
        }

        [Fact]
        public async Task GetBody_Put_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context)
            {
                Method = "PUT",
                Path = new PathString("/api/test")
            };
            request.Headers.Add(_nonceHeader, _nonceValue);
            var body = "woop woop I unit tested the body woop woop";
            request.Body = new MemoryStream();
            using StreamWriter bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.GetBodyAsync();

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal(body, result);
        }
    }
}
