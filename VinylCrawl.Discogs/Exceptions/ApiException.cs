using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VinylCrawl.Discogs.Exceptions
{
    /// <summary>
    ///     Exceptions thrown by the agents
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="apiResponse"></param>
        /// <param name="statusCode"></param>
        public ApiException(string message, string apiResponse, HttpStatusCode statusCode)
            : base(message)
        {
            ApiResponse = apiResponse;
            StatusCode = statusCode;
        }

        /// <summary>
        ///     Returned http status code by the api
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        ///     The content of the response
        /// </summary>
        public string ApiResponse { get; }
    }
}
