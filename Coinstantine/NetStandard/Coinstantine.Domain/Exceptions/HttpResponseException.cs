using System;
using System.Net;
using System.Net.Http;

namespace Coinstantine.Domain.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode => ResponseMessage.StatusCode;

        public HttpResponseMessage ResponseMessage { get; private set; }

        public HttpResponseException(HttpResponseMessage message) : base($"Exception occurred when requesting {message.RequestMessage?.Method} {message.RequestMessage?.RequestUri}: {(int)message.StatusCode} {message.StatusCode} {message.ReasonPhrase}")
        {
            ResponseMessage = message;
        }
    }
}
