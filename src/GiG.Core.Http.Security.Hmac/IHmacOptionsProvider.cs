using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Http.Security.Hmac
{
    public interface IHmacOptionsProvider
    {
        HmacOptions GetHmacOptions();
    }
}
