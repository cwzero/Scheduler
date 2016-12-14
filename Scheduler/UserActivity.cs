using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using static Android.Widget.AdapterView;

namespace Scheduler
{
    [Activity(Label = "UserActivity")]
    public class UserActivity : Activity
    {
        protected static List<string> userList { get; private set; }
        protected ListView userListView { get; set; }

        public static async Task initUsers()
        {
            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/users/");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    string value = new StreamReader(stream).ReadToEnd();
                    userList = JsonConvert.DeserializeObject<List<string>>(value);
                }
            }
        }

        public async void selectItem(object sender, ItemClickEventArgs e)
        {
            string selection = userListView.GetItemAtPosition(e.Position).ToString();

            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/users/" + selection);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    string value = new StreamReader(stream).ReadToEnd();
                    User result = JsonConvert.DeserializeObject<User>(value);

                    UserEditActivity.current = result;
                    StartActivity(typeof(UserEditActivity));
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.User);

            // Create your application here
            userListView = FindViewById<ListView>(Resource.Id.user_list_view);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.User, userList);
            Button createUserButton = FindViewById<Button>(Resource.Id.user_create_button);
            createUserButton.Click += delegate
            {
                UserEditActivity.current = new User();
                StartActivity(typeof(UserEditActivity));
            };
        }
    }
}