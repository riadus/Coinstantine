namespace Coinstantine.Domain.Interfaces
{
    public interface IPathProvider
    {
        string DatabasePath { get; }
        string SecondDatabasePath { get; }
        string ThirdDatabasePath { get; }

        string CachePath { get; }
        string WhitePaperPath { get; }
        string TermsAndServicesPath { get; }
        string PrivacyPolicyPath { get; }
    }
}
