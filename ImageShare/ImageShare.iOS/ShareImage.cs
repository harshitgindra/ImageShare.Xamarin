using System;
using System.Threading.Tasks;
using Foundation;
using ImageShare.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(ShareImage))]
namespace ImageShare.iOS
{
    public class ShareImage : IShareImage
    {
        public async Task Share(string url)
        {
            var imgSource = ImageSource.FromUri(new Uri(url));
            var handler = new ImageLoaderSourceHandler();
            var uiImage = await handler.LoadImageAsync(imgSource);
            var img = NSObject.FromObject(uiImage);
            var mess = NSObject.FromObject("App Name");
            var activityItems = new[] { mess, img };
            var activityController = new UIActivityViewController(activityItems, null);
            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, () => { });
            //return Task.FromResult(0);
        }
    }
}
