using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MauiNotificationv1;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string CHANNELID = "TestChannel";
    internal static readonly int NOTIFICATIONID = 101;
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (Intent.Extras != null)
        {
            foreach (var key in Intent.Extras.KeySet())
            {
                if(key == "NavigationID")
                {
                    string idValue = Intent.Extras.GetString(key);
                    if(Preferences.ContainsKey("NotificationID"))
                    {
                        Preferences.Remove("NotificationID");
                    }
                    Preferences.Set("NotificationID", idValue);
                }
            }
        }

        CreateNotificationChannel();
    }

    private void CreateNotificationChannel()
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var channel = new NotificationChannel(CHANNELID, "Test Notification Channel", NotificationImportance.Default);

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}
