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
using System.Json;
using Newtonsoft.Json;
using static Android.Widget.AdapterView;

namespace Scheduler
{
    [Activity(Label = "EventActivity")]
    public class EventActivity : Activity
    {
        protected static List<string> eventList { get; private set; }
        protected ListView eventListView { get; set; }

        public static async Task initEvents()
        {
            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/events/");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    string value = new StreamReader(stream).ReadToEnd();
                    eventList = JsonConvert.DeserializeObject<List<string>>(value);
                }
            }
        }

        public async void selectItem(object sender, ItemClickEventArgs e)
        {
            string selection = eventListView.GetItemAtPosition(e.Position).ToString();

            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/events/" + selection);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    string value = new StreamReader(stream).ReadToEnd();
                    Event result = JsonConvert.DeserializeObject<Event>(value);

                    EventEditActivity.current = result;
                    StartActivity(typeof(EventEditActivity));
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Event);

            eventListView = FindViewById<ListView>(Resource.Id.event_list_view);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.EventListItem, eventList);
            eventListView.ItemClick += selectItem;

            Button createEventButton = FindViewById<Button>(Resource.Id.event_create_button);
            createEventButton.Click += delegate
            {
                EventEditActivity.current = new Event();
                StartActivity(typeof(EventEditActivity));
            };
        }
    }
}