namespace Coinstantine.Domain.Interfaces
{
    public interface IEndpointProvider
    {
        string Endpoint { get; }
        string WebsiteEndpoint { get; }
        string ClientId { get; }
        string Secret { get; }
    }
}
