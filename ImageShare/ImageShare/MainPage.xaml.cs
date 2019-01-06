using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ImageShare
{
    public partial class MainPage : ContentPage
    {
        private string ImageLink = "https://cdn0.tnwcdn.com/wp-content/blogs.dir/1/files/2018/02/google-pacman-796x419.jpg";
        public MainPage()
        {
            InitializeComponent();
            SampleImage.Source = ImageLink;
            this.Title = "Share Image";
            SetupNavigationBar();
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
                        await DependencyService.Get<IShareImage>().Share(ImageLink);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        await DisplayAlert("Error", "Something went wrong. Please try again later", "OK");
                    }
                })
            };

            ToolbarItems.Add(share);
        }
    }
}
