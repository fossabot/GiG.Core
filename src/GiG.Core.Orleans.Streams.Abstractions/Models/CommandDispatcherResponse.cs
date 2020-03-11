namespace GiG.Core.Orleans.Streams.Abstractions.Models
{
    /// <summary>
    /// The Command Dispatcher Response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandDispatcherResponse<T> where T : class
    {
        /// <summary>
        /// The Data.
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        /// The Error Code.
        /// </summary>
        public string ErrorCode { get; set; }
        
        /// <summary>
        /// The Error Message.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}