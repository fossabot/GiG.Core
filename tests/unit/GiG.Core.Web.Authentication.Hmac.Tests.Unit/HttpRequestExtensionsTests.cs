﻿using GiG.Core.Authentication.Hmac.Abstractions;
using GiG.Core.Web.Authentication.Hmac.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Authentication.Hmac.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HttpRequestExtensionsTests
    {
        private readonly string _nonceHeader;
        private readonly string _nonceValue;
        private const string Body = "woop woop I unit tested the body woop woop";
        
        public HttpRequestExtensionsTests()
        {
            _nonceHeader = Constants.Nonce;
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
            
            request.Body = new MemoryStream();
            await using var bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(Body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.GetBodyAsync();

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal(Body, result);
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
            request.Body = new MemoryStream();
            await using var bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(Body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.GetBodyAsync();

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal(Body, result);
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
            request.Body = new MemoryStream();
            await using var bodyWriter = new StreamWriter(request.Body);
            bodyWriter.Write(Body);
            bodyWriter.Flush();
            request.Body.Position = 0;

            //Act
            var result = await request.GetBodyAsync();

            //Assert
            Assert.Equal(0, request.Body.Position);
            Assert.Equal(Body, result);
        }
    }
}
