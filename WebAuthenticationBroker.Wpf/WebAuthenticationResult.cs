using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAuthenticationBroker.Desktop
{
    public class WebAuthenticationResult
    {
        public string ResponseData { get; }

        /// <summary>
        /// The HTTP Error (404, 400, etc)
        /// </summary>
        public uint ResponseErrorDetail { get; }

        public WebAuthenticationStatus ResponseStatus { get; }

        internal WebAuthenticationResult(string responseData, uint responseErrorDetail, WebAuthenticationStatus status)
        {
            ResponseData = responseData;
            ResponseErrorDetail = responseErrorDetail;
            ResponseStatus = status;
        }
    }
}
