namespace GiG.Core.Models
{
    /// <summary>
    /// A generic Response Model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> where T : class
    {
        private Response()
        {

        }

        /// <summary>
        /// A value to indicate whether the Reponse is successful.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// The Data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// A value to indicate whether the Reponse contains Errors.
        /// </summary>
        public bool IsError => !IsSuccess;

        /// <summary>
        /// The Error Code.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Success opreation.
        /// </summary>
        /// <param name="data">The Data.</param>
        /// <returns></returns>
        public static Response<T> Success(T data) => new Response<T>() { IsSuccess = true, Data = data };

        /// <summary>
        /// Fail operation.
        /// </summary>
        /// <param name="errorCode">The Error Code.</param>
        /// <returns></returns>
        public static Response<T> Fail(string errorCode) => new Response<T>() { IsSuccess = false, ErrorCode = errorCode};
    }
}