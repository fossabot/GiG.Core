using Microsoft.AspNetCore.Authentication;
using System;

namespace GiG.Core.Web.Security.Hmac.Abstractions
{
    public class HmacOptions
    {
        public string Secret { get; set; }
        public string HashAlgorithm { get; set; }
    }
}
