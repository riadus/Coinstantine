using System;

namespace Coinstantine.Domain.Interfaces
{
    public interface IAnalyticsTracker
    {
        void TrackAppPage(string pageName);
        void TrackAppEvent(AnalyticsEventCategory category, string eventToTrack);
        void TrackAppError(AnalyticsEventCategory category, Exception exception);
        void TrackAppError(AnalyticsEventCategory category, string errorMessage);
        void TrackApiCall(string endpoint, long milliseconds);
        void TrackSync(SyncCategory syncCategory, long milliseconds);
    }

    public enum AnalyticsEventCategory
    {
        Sync,
        Buy,
        ValidateTwitter,
        ValidateBct,
        ValidateTelegram,
        ApiCall
    }

    public enum SyncCategory
    {
        UserProfile,
        Translations,
        EnvironmentInfo,
        AirdropDefinitions,
        Documents,
        SmartContractInfo,
        Prices,
        All
    }
}