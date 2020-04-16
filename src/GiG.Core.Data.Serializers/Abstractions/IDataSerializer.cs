namespace GiG.Core.Data.Serializers.Abstractions
{
    /// <summary>
    /// The Data Serializer.
    /// </summary>
    public interface IDataSerializer<T>
    {
        /// <summary>
        /// Returns a model from a string.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns>Generic to define type of model.</returns>
        T Deserialize(string value);

        /// <summary>
        /// Converts a model to a string.
        /// </summary>
        /// <param name="model">The <see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string Serialize(T model);
    }
}