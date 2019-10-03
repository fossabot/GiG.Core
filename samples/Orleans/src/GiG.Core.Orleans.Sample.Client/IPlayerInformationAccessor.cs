using System;

namespace GiG.Core.Orleans.Sample.Client
{
    public interface IPlayerInformationAccessor
    {
        Guid PlayerId { get; }
    }
}