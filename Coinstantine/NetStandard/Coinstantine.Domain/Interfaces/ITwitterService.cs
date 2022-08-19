using System;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces
{
    public interface ITwitterService
    {
        void Authenticate();
        event EventHandler Authenticated;
        TwitterProfile Tweet(string text);
    }
}
