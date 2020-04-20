namespace GiG.Core.Data.Serializers.Abstractions
{
    /// <summary>
    /// The Data Serializer.
    /// </summary>
    public interface IDataSerializer<T>
    {
        /// <summary>
        /// Returns data from a string.
        /// </summary>
        /// <param name="data">The <see cref="string"/>.</param>
        /// <returns>Generic to define type of model.</returns>
        T Deserialize(string data);

        /// <summary>
        /// Converts data to a string.
        /// </summary>
        /// <param name="data">The <see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string Serialize(T data);
    }
}