using System;
using System.IO;
using System.Threading.Tasks;
using GiG.Core.Web.Security.Hmac.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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
        public async Task AsSignatureStringAsync_Get_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context);
            request.Method = "GET";
            request.Path = new PathString("/api/test");
            request.Headers.Add(_nonceHeader, _nonceValue);
            var secret = "test";

            //Act
            var result = await request.AsSignatureStringAsync(_nonceHeader, secret);

            //Assert
            Assert.Equal($"{secret}{_nonceValue}{request.Method}{request.Path.Value}", result);
        }
        [Fact]
        public async Task AsSignatureStringAsync_GetNonceNotAdded_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context);
            request.Method = "GET";
            request.Path = new PathString("/api/test");
            var secret = "test";

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>("nonceHeader", async ()=> await request.AsSignatureStringAsync(_nonceHeader, secret));

        }

        [Fact]
        public async Task AsSignatureStringAsync_Post_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context);

            request.Method = "POST";
            request.Path = new PathString("/api/test");
            request.Headers.Add(_nonceHeader, _nonceValue);
            var secret = "test";
            var body = "woop woop I unit tested the body woop woop";
            request.Body = new MemoryStream();
            StreamWriter bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.AsSignatureStringAsync(_nonceHeader,secret);

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal($"{secret}{_nonceValue}{request.Method}{request.Path.Value}{body}", result);
        }

        [Fact]
        public async Task AsSignatureStringAsync_Patch_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context);

            request.Method = "PATCH";
            request.Path = new PathString("/api/test");
            request.Headers.Add(_nonceHeader, _nonceValue);

            var secret = "test";
            var body = "woop woop I unit tested the body woop woop";
            request.Body = new MemoryStream();
            StreamWriter bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.AsSignatureStringAsync(_nonceHeader,secret);

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal($"{secret}{_nonceValue}{request.Method}{request.Path.Value}{body}", result);
        }

        [Fact]
        public async Task AsSignatureStringAsync_Put_ReturnsSecretMethodUrl()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var request = new DefaultHttpRequest(context);

            request.Method = "PUT";
            request.Path = new PathString("/api/test"); 
            request.Headers.Add(_nonceHeader, _nonceValue);

            var secret = "test";
            var body = "woop woop I unit tested the body woop woop";
            request.Body = new MemoryStream();
            StreamWriter bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.AsSignatureStringAsync(_nonceHeader,secret);

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal($"{secret}{_nonceValue}{request.Method}{request.Path.Value}{body}", result);
        }
    }
}
