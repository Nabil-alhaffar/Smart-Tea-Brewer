using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms;
using Tizen.Applications.Notifications;
using Tizen.Applications;

namespace TeaPot1
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
         
       
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
        
            // Handle when your app sleeps
            MessagingCenter.Send (this, "sleep");

            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            MessagingCenter.Send(this, "resume");

            // Handle when your app resumes
        }
        //  protected override void OnTick(TimeEventArgs time)
        //{
        //  /// Called at least once per second
        ///// Draw a normal watch with the hour, minute, and second info
        //
        //base.OnTick(time);
        // }

    }
}
