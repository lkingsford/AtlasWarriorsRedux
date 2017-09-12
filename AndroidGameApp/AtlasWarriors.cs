using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using System.IO;

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
            var metrics = Resources.DisplayMetrics;
            var g = new AndroidGameApp(metrics, GetAssetStream);
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }

        /// <summary>
        /// Get asset stream from Assets
        /// </summary>
        /// <param name="filename">Filename to open</param>
        /// <returns>Stream containing asset</returns>
        protected Stream GetAssetStream(string filename)
        {
            return Assets.Open(filename);
        }
    }
}

