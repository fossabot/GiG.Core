namespace GiG.Core.Models
{
    /// <summary>
    /// A generic Response Model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> where T : class
    {
        /// <summary>
        /// The Data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// A value to indicate whether the Reponse contains Errors.
        /// </summary>
        public bool IsError => !IsSuccess;

        /// <summary>
        /// A value to indicate whether the Reponse is successful.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// The Error Code.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Success method.
        /// </summary>
        /// <param name="data">The Data.</param>
        /// <returns></returns>
        public static Response<T> Success(T data) => new Response<T>() { IsSuccess = true, Data = data };

        /// <summary>
        /// Fail method.
        /// </summary>
        /// <param name="errorCode">The Error Code.</param>
        /// <returns></returns>
        public static Response<T> Fail(string errorCode) => new Response<T>() { IsSuccess = false, ErrorCode = errorCode};
    }
}