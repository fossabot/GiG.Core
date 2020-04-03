namespace GiG.Core.Orleans.Streams.Abstractions.Models
{
    /// <summary>
    /// The Command Dispatcher Response.
    /// </summary>
    /// <typeparam name="T">Type of Successful payload.</typeparam>
    public class CommandDispatcherResponse<T> where T : class
    {
        /// <summary>
        /// The Data.
        /// </summary>
        public T Data { get; }
        
        /// <summary>
        /// The Error Code.
        /// </summary>
        public string ErrorCode { get; }
        
        /// <summary>
        /// The Error Message.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Command was processed successfully.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// There was an error while processing the command.
        /// </summary>
        public bool IsError => !IsSuccess;

        /// <summary>
        /// Constructor for Successful data.
        /// </summary>
        /// <param name="data">The data.</param>
        public CommandDispatcherResponse(T data)
        {
            IsSuccess = true;
            Data = data;
        }
        
        /// <summary>
        /// Constructor for Errors.
        /// </summary>
        /// <param name="errorCode">The Error Code.</param>
        /// <param name="errorMessage">The Error Message.</param>
        public CommandDispatcherResponse(string errorCode, string errorMessage = null)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}