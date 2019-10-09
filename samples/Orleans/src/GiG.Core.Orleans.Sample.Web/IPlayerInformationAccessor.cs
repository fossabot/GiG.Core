using System;

namespace GiG.Core.Orleans.Sample.Web
{
    public interface IPlayerInformationAccessor
    {
        Guid PlayerId { get; }
    }
}