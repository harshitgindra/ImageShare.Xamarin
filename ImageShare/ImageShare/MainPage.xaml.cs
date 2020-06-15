using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageShare
{
    public partial class MainPage : ContentPage
    {
        private string ImageLink = "https://www.hexacta.com/wp-content/uploads/2016/03/xamarin.jpg";

        public MainPage()
        {
            InitializeComponent();
            SampleImage.Source = ImageLink;
            this.Title = "Share Image";
            SetupNavigationBar();
        }

        public byte[] GetBytes(string url)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadData(url);
            }
        }

        private void SetupNavigationBar()
        {
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
    }
}