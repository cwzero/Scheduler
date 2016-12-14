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
    [Activity(Label = "GroupActivity")]
    public class GroupActivity : Activity
    {
        protected static List<string> groupList { get; private set; }
        protected ListView groupListView { get; set; }

        public static async Task initGroups()
        {
            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/groups/");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    string value = new StreamReader(stream).ReadToEnd();
                    groupList = JsonConvert.DeserializeObject<List<string>>(value);
                }
            }
        }

        public async void selectItem(object sender, ItemClickEventArgs e)
        {
            string selection = groupListView.GetItemAtPosition(e.Position).ToString();

            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/groups/" + selection);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    string value = new StreamReader(stream).ReadToEnd();
                    Group result = JsonConvert.DeserializeObject<Group>(value);

                    GroupEditActivity.current = result;
                    StartActivity(typeof(GroupEditActivity));
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Group);

            groupListView = FindViewById<ListView>(Resource.Id.group_list_view);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.Group, groupList);
            Button createGroupButton = FindViewById<Button>(Resource.Id.group_create_button);
            createGroupButton.Click += delegate
            {
                GroupEditActivity.current = new Group();
                StartActivity(typeof(GroupEditActivity));
            };
        }
    }
}