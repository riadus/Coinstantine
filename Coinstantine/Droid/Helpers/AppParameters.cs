using MvvmCross;
using MvvmCross.Platforms.Android;
using Plugin.CurrentActivity;

namespace Coinstantine.Droid.Helpers
{
    public static class AppParameters
    {
        public static string GetAppParameter(string key)
        {
            if (TryGetIdentifier(key, out Android.Content.Context context, out int keyIdentifier))
            {
                return context.GetString(keyIdentifier);
            }
            return string.Empty;
        }

        private static bool TryGetIdentifier(string key, out Android.Content.Context context, out int keyIdentifier)
        {
            context = CrossCurrentActivity.Current.Activity; ;
            var packageName = context?.PackageName;

            keyIdentifier = context?.Resources.GetIdentifier(key, "string", packageName) ?? 0;
            return keyIdentifier != 0;
        }

        public static bool ContainsKey(string key)
        {
            return TryGetIdentifier(key, out Android.Content.Context context, out int keyIdentifier);
        }
    }
}
