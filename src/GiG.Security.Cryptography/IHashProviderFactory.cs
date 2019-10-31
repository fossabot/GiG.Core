namespace GiG.Core.Security.Cryptography
{
    public interface IHashProviderFactory
    {
        IHashProvider GetHashProvider(string hashAlgorithm);
    }
}
