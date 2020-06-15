# ImageShare.Xamarin
###### This app allows you to build Xamarin forms app to share media to different apps. This repository is an example to share images on Android and iOS apps.

## Xamarin.Portable

Here we'll be using [Xamarin Essentials](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/dependency-service/introduction) to support different implementation for Android and iOS

In your XAML page, add the following ToolBarItem with a Command associated

```
            var share = new ToolbarItem
            {
                Text = "Share",
                Command = new Command(async () =>
                {
                    try
                    {
                        string filename = "temp.jpg";
                        //***
                        //*** Get the file name from the link
                        //***
                        Uri uri = new Uri(ImageLink);
                        if (uri.IsFile)
                        {
                            filename = System.IO.Path.GetFileName(uri.LocalPath);
                        }

                        //***
                        //*** Build a temp path for the file
                        //***
                        string tempFileName = Path.Combine(FileSystem.CacheDirectory, filename);

                        //***
                        //*** Delete the file if exist in the system
                        //*** 
                        if (File.Exists(tempFileName))
                        {
                            File.Delete(tempFileName);
                        }

                        //***
                        //*** Write the file to the directory
                        //***
                        File.WriteAllBytes(tempFileName, GetBytes(ImageLink));

                        //***
                        //*** Share file
                        //***
                        await Share.RequestAsync(new ShareFileRequest()
                        {
                            File = new ShareFile(tempFileName),
                            Title = "Sharing image",
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        await DisplayAlert("Error", $"Something went wrong. {ex.Message}", "OK");
                    }
                })
            };

            ToolbarItems.Add(share);
        }
```

Here's the method to read url into bytes

```

        public byte[] GetBytes(string url)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadData(url);
            }
        }

```

## Additional notes
Refer to this [link](https://docs.microsoft.com/en-us/xamarin/essentials/share?tabs=android) for additional information

## License
This project is open source.

##### Thank you
