namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// The Data Serializer used to fetch data from source.
    /// </summary>
    public interface IDataSerializer<out T>
    {
        /// <summary>
        /// Returns a Model from String.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns>Generic to define type of model.</returns>
        T GetFromString(string value);
    }
}
