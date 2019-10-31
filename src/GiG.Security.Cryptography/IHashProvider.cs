namespace GiG.Core.Security.Cryptography
{
    public interface IHashProvider
    {
        string Name { get; }
        string Hash(string message);
    }
}
