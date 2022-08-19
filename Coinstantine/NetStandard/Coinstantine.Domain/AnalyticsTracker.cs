using System;
using System.Collections.Generic;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Coinstantine.Domain
{
	[RegisterInterfaceAsDynamic]
	public class AnalyticsTracker : IAnalyticsTracker
    {
        private string GetRange(long milliseconds)
        {
            if(milliseconds < 100)
            {
                return "< 100 ms";
            }
            if(milliseconds < 500)
            {
                return "< 500 ms";
            }
            if(milliseconds < 1000)
            {
                return "< 1000 ms";
            }
            if(milliseconds < 2000)
            {
                return "< 2000 ms";
            }
            if (milliseconds < 5000)
            {
                return "< 5000 ms";
            }
            if (milliseconds < 10000)
            {
                return "< 10000 ms";
            }
            if (milliseconds < 15000)
            {
                return "< 15 s";
            }
            if (milliseconds < 20000)
            {
                return "< 20 s";
            }
            if (milliseconds < 30000)
            {
                return "< 30 s";
            }
            if (milliseconds < 45000)
            {
                return "< 45 s";
            }
            if (milliseconds < 60000)
            {
                return "< 60 s";
            }
            if (milliseconds < 90000)
            {
                return "< 90 s";
            }
            if (milliseconds < 120000)
            {
                return "< 2 min";
            }
            return "> 2 min";
        }
        public void TrackApiCall(string endpoint, long milliseconds)
        {
            Analytics.TrackEvent($"ApiCall - {endpoint}", new Dictionary<string, string> { { endpoint, GetRange(milliseconds) } });
        }

        public void TrackSync(SyncCategory syncCategory, long milliseconds)
        {
            Analytics.TrackEvent($"Syncing - {syncCategory}", new Dictionary<string, string> { { syncCategory.ToString(), GetRange(milliseconds) } });
        }

        public void TrackAppError(AnalyticsEventCategory category, Exception exception)
        {
            Crashes.TrackError(exception, new Dictionary<string, string> { { "Category", category.ToString() } });
        }

        public void TrackAppError(AnalyticsEventCategory category, string errorMessage)
        {
            Analytics.TrackEvent("Error", new Dictionary<string, string> { { category.ToString(), errorMessage } });
        }

        public void TrackAppEvent(AnalyticsEventCategory category, string eventToTrack)
        {
            Analytics.TrackEvent("Event", new Dictionary<string, string> { { category.ToString(), eventToTrack } });
        }

        public void TrackAppPage(string pageName)
        {
            Analytics.TrackEvent("Page visited", new Dictionary<string, string> { { "Page", pageName } });
        }
    }
}
