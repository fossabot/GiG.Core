namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// The Data Serializer used to fetch data from source.
    /// </summary>
    public interface IDataSerializer<T>
    {
        /// <summary>
        /// Returns a model from a string.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns>Generic to define type of model.</returns>
        T GetFromString(string value);

        /// <summary>
        /// Converts a model from a string.
        /// </summary>
        /// <param name="model">The <see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string ConvertToString(T model);
    }
}
