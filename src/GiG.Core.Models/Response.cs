namespace GiG.Core.Models
{
    /// <summary>
    /// A generic Response Model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        /// <summary>
        /// A value to indicate whether the Response is successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The Data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// A value to indicate whether the Response contains Errors.
        /// </summary>
        public bool IsError => !IsSuccess;

        /// <summary>
        /// The Error Code.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Success operation.
        /// </summary>
        /// <param name="data">The Data.</param>
        /// <returns></returns>
        public static Response<T> Success(T data) => new Response<T> { IsSuccess = true, Data = data };

        /// <summary>
        /// Fail operation.
        /// </summary>
        /// <param name="errorCode">The Error Code.</param>
        /// <returns></returns>
        public static Response<T> Fail(string errorCode) => new Response<T> { IsSuccess = false, ErrorCode = errorCode};

        /// <summary>
        /// Fail operation.
        /// </summary>
        /// <param name="errorCode">The Error Code.</param>
        /// <param name="data">The Data.</param>
        /// <returns></returns>
        public static Response<T> Fail(T data, string errorCode) => new Response<T> { IsSuccess = false, ErrorCode = errorCode, Data = data };
    }
}