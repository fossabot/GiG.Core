using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Http.Security.Hmac
{
    public class HmacOptions
    {
        public string Hash { get; set; }
        public string Secret { get; set; }
    }
}
