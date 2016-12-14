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

namespace Scheduler
{
    [Activity(Label = "GroupEditActivity")]
    public class GroupEditActivity : Activity
    {
        public static Group current { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Button saveButton = FindViewById<Button>(Resource.Id.group_save_button);
            saveButton.Click += createGroup;
        }

        public async void createGroup(object sender, EventArgs e)
        {
            EditText editName = FindViewById<EditText>(Resource.Id.edit_group_name);
            EditText editDesc = FindViewById<EditText>(Resource.Id.edit_group_desc);
            EditText editOwner = FindViewById<EditText>(Resource.Id.edit_group_owner);

            Group newGroup = new Scheduler.Group();
            newGroup.name = editName.Text;
            newGroup.desc = editDesc.Text;
            newGroup.owner = editOwner.Text;

            await createGroup(current, newGroup);
        }

        public static async Task createGroup(Group oldGroup, Group newGroup)
        {
            string addUri = "";
            if (!string.IsNullOrWhiteSpace(oldGroup.name))
            {
                addUri = oldGroup.name;
            }
            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/groups/" + addUri);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(requestStream))
                {
                    string body = JsonConvert.SerializeObject(newGroup);
                    writer.WriteLine(body);
                    writer.Flush();
                }
            }

            request.ContentType = "application/json";
            request.Method = "POST";

            using (WebResponse response = await request.GetResponseAsync())
            {
            }
        }
    }
}