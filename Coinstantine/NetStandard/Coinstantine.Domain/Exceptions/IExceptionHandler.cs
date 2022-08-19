using System;
using System.Net;
using System.Threading.Tasks;

namespace Coinstantine.Domain.Exceptions
{
    public interface IExceptionHandler
    {
        T Throw<T>(Exception e);
        T ThrowApiError<T>(HttpStatusCode statusCode, Func<T> retryFunction);
        Task<T> ThrowApiErrorAsync<T>(HttpStatusCode statusCode, Func<Task<T>> retryFunction);
    }
}
