using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tizen.Wearable.CircularUI.Forms;
using Tizen.Applications.Notifications;
using Tizen.Applications;
namespace TeaPot1
{


    /// </summary>

    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPage : Xamarin.Forms.Application
    {
        public MainPage()
        {
            //MainNavigation.PushAsync(new SplashScreen()  );
            InitializeComponent();
        }
        /// <summary>
        /// Called when item is tapped
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="args">Argument of ItemTappedEventArgs</param>
        public void OnItemTapped(object sender, ItemTappedEventArgs args)
        {
            if (args.Item == null)
            {
                return;
            }
            var dest = args.Item as Component;
            if (dest != null && dest.Class != null)
            {
                Type pageType = dest.Class;
                var page = Activator.CreateInstance(pageType) as Page;
                NavigationPage.SetHasNavigationBar(page, false);
                MainNavigation.PushAsync(page as Page);
            }
        }
    }
}