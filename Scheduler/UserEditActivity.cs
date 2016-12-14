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
    [Activity(Label = "UserEditActivity")]
    public class UserEditActivity : Activity
    {
        public static User current { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Button saveButton = FindViewById<Button>(Resource.Id.user_save_button);

            saveButton.Click += createUser;
        }

        public async void createUser(object sender, EventArgs e)
        {
            EditText editName = FindViewById<EditText>(Resource.Id.edit_user_name);
            EditText editPassword = FindViewById<EditText>(Resource.Id.edit_user_password);

            User newUser = new Scheduler.User();
            newUser.name = editName.Text;
            newUser.password = editPassword.Text;

            await createUser(current, newUser);
        }

        public static async Task createUser(User oldUser, User newUser)
        {
            string addUri = "";
            if (!string.IsNullOrWhiteSpace(oldUser.name))
            {
                addUri = oldUser.name;
            }
            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/users/" + addUri);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(requestStream))
                {
                    string body = JsonConvert.SerializeObject(newUser);
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