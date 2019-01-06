using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using ImageShare.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareImage))]
namespace ImageShare.Droid
{
    public class ShareImage : Activity, IShareImage
    {
        public Task Share(string url)
        {
            var path = Android.OS.Environment.GetExternalStoragePublicDirectory("Temp");

            if (!File.Exists(path.Path))
            {
                Directory.CreateDirectory(path.Path);
            }

            string absPath = path.Path + "tempfile.jpg";
            File.WriteAllBytes(absPath, GetBytes(url));

            var _context = Android.App.Application.Context;

            Intent sendIntent = new Intent(global::Android.Content.Intent.ActionSend);

            sendIntent.PutExtra(global::Android.Content.Intent.ExtraText, "Application Name");

            sendIntent.SetType("image/*");

            sendIntent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + absPath));
            _context.StartActivity(Intent.CreateChooser(sendIntent, "Sharing"));
            return Task.FromResult(0);
        }

        public static byte[] GetBytes(string url)
        {
            byte[] arry;
            using (var webClient = new WebClient())
            {
                arry = webClient.DownloadData(url);
            }
            return arry;
        }
    }
}
