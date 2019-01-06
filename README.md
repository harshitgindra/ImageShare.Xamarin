# ImageShare.Xamarin
###### This app allows you to build Xamarin forms app to share media to different apps. This repository is an example to share images on Android and iOS apps

## Xamarin.Portable

Here we'll be using [DependencyService](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/dependency-service/introduction) to support different implementation for Android and iOS

```
    public interface IShareImage
    {
        Task Share(string url);
    }
```

In your XAML page, add the following ToolBarItem with a Command associated

```
            var share = new ToolbarItem
            {
                Text = "Share",
                Command = new Command(async () =>
                {
                    try
                    {
                        await DependencyService.Get<IShareImage>().Share("Your Media Link");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        await DisplayAlert("Error", "Something went wrong. Please try again later", "OK");
                    }
                })
            };

            ToolbarItems.Add(share);
```

## Xamarin.Android

For Android Implementation, first we need to temporarily save the image on the device, and then we can share it using [Intent](https://developer.xamarin.com/api/type/Android.Content.Intent/)

##### Saving the Image on the device
```
            var path = Android.OS.Environment.GetExternalStoragePublicDirectory("Temp");

            if (!File.Exists(path.Path))
            {
                Directory.CreateDirectory(path.Path);
            }

            string absPath = path.Path + "tempfile.jpg";
            File.WriteAllBytes(absPath, GetBytes(url));

```

##### Now creating a Intent to share the image 
```
            var _context = Android.App.Application.Context;

            Intent sendIntent = new Intent(global::Android.Content.Intent.ActionSend);

            sendIntent.PutExtra(global::Android.Content.Intent.ExtraText, "Application Name");

            sendIntent.SetType("image/*");

            sendIntent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + absPath));
            _context.StartActivity(Intent.CreateChooser(sendIntent, "Sharing"));
            
```
##### AndroidManifest.XML
To get it to work, we need to allow these permissions for our application. 
```
      <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
      <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
      <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
  
  ```
  
  ##### File Provider Access
  If your targetSdkVersion >= 24, then we have to use **FileProvider** class to give access to the particular file or folder to make them accessible for other apps. For more info, refer to this [post](https://stackoverflow.com/questions/38200282/android-os-fileuriexposedexception-file-storage-emulated-0-test-txt-exposed) There are different ways of doing it but this is how I've done. 
  Add the following lines in **MAINACTIVITY.CS** class
  ```
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
  
  ```
  
## Xamarin.iOS
This code was taken from the following [link](https://stackoverflow.com/questions/44230026/sharing-an-image-using-xamarin-forms) on StackOverflow with minor changes

```
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
            
```

##### Info.plist
In case you want to save the media/image to your device, do not forget to set the following Property on Info.plist

**Privacy - Photo Library Additions Usage Description**


##### Thank you
