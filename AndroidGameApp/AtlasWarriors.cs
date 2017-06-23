using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace AndroidGameApp
{
    [Activity(Label = "Atlas Warriors Redux"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Portrait
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class AtlasWarriors : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new AndroidGameApp();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

