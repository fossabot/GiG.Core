namespace GiG.Core.Security.Cryptography
{
    /// <summary>
    /// Hash Provider to hash a string.
    /// </summary>
    public interface IHashProvider
    {
        /// <summary>
        /// The name of the <see cref="IHashProvider"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Hashes the <see cref="string"/> argument.
        /// </summary>
        /// <param name="message"><see cref="string"/> to hash.</param>
        /// <returns></returns>
        string Hash(string message);
    }
}
