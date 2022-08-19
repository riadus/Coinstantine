using System;
using System.Net;
using System.Threading.Tasks;
using Coinstantine.Domain.Exceptions;

namespace Coinstantine.Core.Services
{
    public class ExceptionHandler : IExceptionHandler
    {
        public T Throw<T>(Exception e)
        {
            throw new NotImplementedException();
        }

        public T ThrowApiError<T>(HttpStatusCode statusCode, Func<T> retryFunction)
        {
            throw new NotImplementedException();
        }

        public Task<T> ThrowApiErrorAsync<T>(HttpStatusCode statusCode, Func<Task<T>> retryFunction)
        {
            throw new NotImplementedException();
        }
    }
}
