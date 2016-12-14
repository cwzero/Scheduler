using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Scheduler
{
    [Activity(Label = "Scheduler", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Button eventButton = FindViewById<Button>(Resource.Id.event_activity_button);
            eventButton.Click += OpenEventActivity;

            Button groupButton = FindViewById<Button>(Resource.Id.group_activity_button);
            groupButton.Click += OpenGroupActivity;

            Button userButton = FindViewById<Button>(Resource.Id.user_activity_button);
            userButton.Click += OpenUserActivity;
        }

        protected async void OpenEventActivity(object sender, EventArgs e)
        {
            await EventActivity.initEvents();
            StartActivity(typeof(EventActivity));
        }

        protected async void OpenGroupActivity(object sender, EventArgs e)
        {
            await GroupActivity.initGroups();
            StartActivity(typeof(GroupActivity));
        }

        protected async void OpenUserActivity(object sender, EventArgs e)
        {
            await UserActivity.initUsers();
            StartActivity(typeof(UserActivity));
        }
    }
}

