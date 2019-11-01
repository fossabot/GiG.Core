namespace GiG.Core.Security.Cryptography
{
    /// <summary>
    /// <see cref="IHashProviderFactory"/> for <see cref="IHashProvider"/>.
    /// </summary>
    public interface IHashProviderFactory
    {
        /// <summary>
        /// Gets the <see cref="IHashProvider"/> for the <paramref name="hashAlgorithm"/> argument.
        /// </summary>
        /// <param name="hashAlgorithm">The unique name of the <see cref="IHashProvider"/>.</param>
        /// <returns></returns>
        IHashProvider GetHashProvider(string hashAlgorithm);
    }
}
