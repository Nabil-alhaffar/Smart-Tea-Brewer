using System;
using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;

namespace TeaPot1.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleDateTimeList : CirclePage
    {
        public CircleDateTimeList()
        {
            InitializeComponent();
        }
        public void OnItemTapped(object sender, ItemTappedEventArgs args)
        {
            if (args.Item == null)
            {
                return;
            }
            var desc = args.Item as Component;
            Console.WriteLine($"OnItemTapped desc.Class:{desc.Class}");
            if (desc != null && desc.Class != null)
            {
                Type pageType = desc.Class;
                // Create page and push it to navigation stack
                var page = Activator.CreateInstance(pageType) as Page;
                NavigationPage.SetHasNavigationBar(page, false);
                Navigation.PushAsync(page as Page);
            }

        }
    }
}